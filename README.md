# Universal Text
Unity/C# repository for Universal Text. This is where we house the developer-facing features of the Universal Text Package.
Check out the [Technical Specification](https://docs.google.com/document/d/1ZENwW2QDMECsh4XjdapVCEHr-pyB22QbAG2YkK9n5k4/edit#heading=h.sqd56vvgynf), and here's a [demo](https://file.notion.so/f/f/bfe57eb7-4a4c-4be7-a345-654e14f2a535/4989b819-4938-457e-8ac5-ea67356e6b78/8mb.video-sht-Hlr0Woeg_(1).mp4?table=block&id=1c2bc072-402f-804f-9b72-e98b6c2c0091&spaceId=bfe57eb7-4a4c-4be7-a345-654e14f2a535&expirationTimestamp=1743026400000&signature=Ic5gRzBXysrAtiRIFsrmB5twiOKZ8JBMmAMeX9UnGx4&downloadName=8mb.video-sht-Hlr0Woeg+%281%29.mp4) of what we accomplished at the end of the F24 term.
# Setup
## Unity 2022.3.24f1
Unity is the editor that we use to create 3D/XR applications. We will be using it as our engine for the project. 
1. Download the [Unity Hub](https://unity.com/download), then use it to install Editor version 2022.3.24f1.
2. Create a Unity account to go along with your install
3. [Clone](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository) the repository.
4. Open Unity Hub, choose "Add" and select the cloned folder. This will add the project to your Unity Hub, where you can then launch the project.
## Oculus PC App
If you want to run the project on a Quest headset to test its functionality, you will need to install the [Oculus PC App](https://www.meta.com/help/quest/articles/headsets-and-accessories/oculus-rift-s/install-app-for-link/). Note that this only supports Windows PCs. To connect your Quest headset to your PC, you can use a Link Cable (requires USB3.0 port on your computer) or Air Link (requires strong Wifi connection that Quest can independently connect to: eduroam is not compatible).
Once connected to your PC, launch Link Mode on your Quest if it's not automatically started. In Unity, ensure Oculus is added to the list of active loaders (Edit > Project Settings > Meta XR). Then, you can simply start Play Mode and the app will be displayed to the Quest headset.
