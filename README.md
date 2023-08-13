# Chirp - Caption System for Unity XR Projects

⚠️ This is an alpha release of this package. If you encounter any bugs or problems, please open a GitHub issue.

Chirp is an open-source caption system for extended reality (XR) projects developed in Unity. With Chirp, you can easily integrate captions and subtitles into your XR apps to enhance accessibility.

## Getting Started

Chirp can be easily integrated into any existing Unity project that uses Unity `2020.3` or later.

### Install the Chirp package

1. In the Unity editor, navigate to **Window > Package Manager > Add (Plus symbol) > Add package from git URL**.
2. Enter the following git URL `https://github.com/XR-Access-Initiative/chirp-captions.git`, then press Add.
3. If you run into any issues with adding the package, refer to Unity’s official documentation on how to [install packages from a git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html).

### Add the Chirp prefab

1. From **Packages > Chirp Captions > Runtime > Prefabs**, locate the prefab called **ChirpCaptions**. Drag this into the hierarchy of your scene.
2. The caption system has to have a reference to your main Camera component. Drag the GameObject containing the **Main Camera** and **AudioListener** components into the public fields called "Main Camera" and "Main Audio Listener" in the _CaptionSystem_ script attached to the **ChirpCaptions** prefab.

### Create a new layer for captions

Chirp renders captions on a separate Unity [layer](https://docs.unity3d.com/Manual/Layers.html). This layer has to be manually created in your project.

1. To create a new layer for captions, go to **Edit > Project Settings**, then select the Tags and Layers category.
2. Create a new layer called `Captions` in one of the empty User Layers.

### The CaptionSource component

Chirp's CaptionSource script can be assigned to objects that will provide captions. This is similar to how an AudioSource is added to objects that produce audio.

1. Add a CaptionSource by using **Add Component** button in a GameObject's inspector.
2. To trigger a caption from this CaptionSource, you have to call a public method on this script such as ShowTimedCaption(), similar to how you would call PlayOneShot() on an AudioSource.

### Caption Options

Chirp is a customizable caption system that provides many options to both developers and users. Refer to the [Chirp wiki](https://github.com/XR-Access-Initiative/chirp-captions/wiki) for more information on these options.

## About XR Access's Prototype for the People Project

Chirp is developed as part of XR Access's Prototype for the People project. [XR Access](https://xraccess.org/) is a community committed to making virtual, augmented, and mixed reality (XR) accessible to people with disabilities. The [Prototype for the People](https://xraccess.org/workstreams/prototype-for-the-people/) project aims to create code solutions that promote inclusivity and equal access to XR experiences for all individuals.

## Development Status

Chirp is still under active development. We welcome contributions, bug reports, and feature requests from the XR community to improve Chirp.

## License

Chirp is released under the [MIT License](LICENSE). Feel free to use, modify, and distribute the code, subject to the terms and conditions outlined in the license.

---

_Note: The above information is subject to change as Chirp is still under development._
