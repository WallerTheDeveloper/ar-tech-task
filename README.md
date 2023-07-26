
# AR Developer Technical Challenge #2 - Golosov Danylo



## Requirements

In order to build and launch app successfuly you will need any machine with MacOs installed, IOS device and Apple Developer Account (for IOS build)

## Used Tools

- Unity 2023.1.4f1

- Git Large File Storage

- Git 2.41.0.windows.2

- XCode 14.3.1

- Apple Developer Account (for IOS build)

## Dependencies

- Unity 2023.1.4f1

- Git version 1.8.3.1 or later

- Git Large File Storage (optionally)

- Apple Developer Account (for IOS build)


# Installation Steps

## 1. Before you start

Before launching project you will need to install Unity 2023.1.4f1 on your computer

## 2. Clone project

Clone project anywhere convinient for you with following command in terminal

```bash
  git clone https://github.com/WallerTheDeveloper/ar-tech-task.git
```

## 3. Launch project

Launch project from UnityHub app

## 4. Switch Platform to IOS

You need to switch build platform in **Build Settings** menu

Inside project go to 
```bash
  File > Build Settings
```
Choose **IOS** platform and check **Development Build** box.

**IMPORTANT NOTE:** 

It's crucial to check **Development Build** box. This will allow you to mark the build as a test build and not intended to be deployed to the App Store.

Also note that the build will only be available for 48 hours afterwards.

## 5. Plug-in your IOS device

Plug-in your IOS device to MacOs device and click on **Build and Run** button

## 6. Place build in project

In pop-up create new folder in project root and choose it as build destination (suggested folder name: Builds)

## 7. Build project in XCode

*During build XCode will be automaticaly launched. It might prompt error, which is related to app signing and looks like this*

```bash
  Signing for "Unity-iPhone" requires a development team. Select a development team in the Signing & Capabilities editor.
```
Signing for IOS app requires a development team and it won't let you build project without Apple Developer Account

**To sign app, follow these steps:**

- Click on error

- Go to following
```bash
  Signing & Capabilities > Team
```

and assign any Apple Developer Account
# Possible issues

## 1. Project git clone problem

When cloning project for the first time, there might be an error, indicating problem with **AR Google Extension Package** importing. The problem is probably with from google side.

- Solution
*Delete pulled earlier project, and pull it over again. If problem is not disappearing, try and pull again.*

## 2. ImportFBX errors in editor

You may encounter a problem importing some resources. Because the project uses Git Large File Storage, it stores resources that weigh more, that git can handle. So in order to pull those resources, you need to install git lfs.

- Solution
1. *git-lfs requires git version 1.8.3.1 or later. You can check the version you have by running git --version, and update if required.*
2. *Make sure you have Homebrew installed on your MacOs machine*

```bash
  $ brew update
  $ brew install git-lfs
```

3. *Finally, run*
```bash
git-lfs install 
```
*to install git-lfs on your system. You can always run git-lfs uninstall to uninstall.*

4. Pull project files by running next command

```bash
git lfs pull
```

