To build the DockerFile located inside src enter the folder and launch terminal. Run "docker -t gamifying-tasks ."


Additional setup is required to get the Google Credentials running as you will need the GOOGLE_APPLICATION_CREDENTIALS set as an Enviroment Variable. Once done open a terminal in the src folder and run "udo docker run -p 80:80 -v "$HOME/.config/gcloud/application_default_credentials.json":/gcp/creds.json:ro --env GOOGLE_APPLICATION_CREDENTIALS=/gcp/creds.json gamifying-tasks"
