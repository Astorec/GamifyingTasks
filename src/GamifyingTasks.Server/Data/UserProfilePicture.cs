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

namespace GamifyingTasks.Data
{
    public static class UserProfilePicture
    {
        public static async Task UploadNewPFP(IBrowserFile file)
        {
            MemoryStream ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);
            var bytes = ms.ToArray();


            using (var image = SixLabors.ImageSharp.Image.Load(bytes))
            {
                if (image.Width > 200 || image.Height > 200)
                {
                    var newImage = image.Clone(x => x.Resize(200, 200));

                    ms = new MemoryStream();
                    newImage.SaveAsPng(ms);
                    ms.Position = 0;
                    var task = new FirebaseStorage
                    ("hons-project-f5a1e.appspot.com",
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(DBCore.CurrentUser().GetIdTokenAsync().Result),
                        ThrowOnCancel = true,
                    })
                                .Child("ProfilePictures")
                                .Child(DBCore.CurrentLocalUser.Uid)
                                .PutAsync(ms);
                    var downloadUrl = await task;
                      Console.WriteLine($"Upload completed. Download URL: {downloadUrl}"); // Debugging output
                    DBCore.CurrentLocalUser.PfpUrl = downloadUrl;
                    await DBCore.UpdateUser(DBCore.CurrentLocalUser);
                }
            }

        }

        public static async Task GetPFP()
        {

        }

    }
}
