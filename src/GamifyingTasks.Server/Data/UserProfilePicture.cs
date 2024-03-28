using System.Drawing;
using GamifyingTasks.Firebase.DB;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using Firebase.Storage;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Routing.Constraints;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using FirebaseAdmin.Auth;
using GamifyingTasks.Firebase.DB.Interfaces;

namespace GamifyingTasks.Data
{
    public static class UserProfilePicture
    {  
        /// <summary>
        /// UploadNewPFP uploads a new profile picture to the database
        /// </summary>
        /// <param name="file"></param>
        /// <param name="_users"></param>
        /// <returns></returns>
        public static async Task UploadNewPFP(IBrowserFile file, IUsers _users)
        {
            // Create a memory stream to store the image
            MemoryStream ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);
            var bytes = ms.ToArray();

            // Load the image
            using (var image = SixLabors.ImageSharp.Image.Load(bytes))
            {
                // Resize the image if it is too large
                if (image.Width > 200 || image.Height > 200)
                {
                    var newImage = image.Clone(x => x.Resize(200, 200));

                    // Save the image as a PNG
                    ms = new MemoryStream();
                    newImage.SaveAsPng(ms);
                    ms.Position = 0;

                    // Upload the image to the database
                    var task = new FirebaseStorage
                    ("hons-project-f5a1e.appspot.com",
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(_users.GetUser().Uid),
                        ThrowOnCancel = true,
                    })
                                .Child("ProfilePictures")
                                .Child(_users.GetUser().Uid)
                                .PutAsync(ms);
                    var downloadUrl = await task;
                      Console.WriteLine($"Upload completed. Download URL: {downloadUrl}"); // Debugging output
                    _users.GetUser().PfpUrl = downloadUrl;
                    await _users.UpdateUser(_users.GetUser());
                }
            }

        }
    }
}
