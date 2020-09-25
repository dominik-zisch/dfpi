# Perception Neuron + Visual Effects Graph

![img1](img/image.png)
## Authors
George Adamopoulos

## Requirements
* Unity 2020.1 +
* High Definition Pipeline package (HDRP) 8.2.0

## Dependencies
The project uses the solution provided by [Keijiro Takahashi](https://github.com/keijiro/Smrvfx) in order to bake mesh information as textures in real-time. The package is fetched automatically.

## How To
The project plays back a pre-recorded animation by default. To switch to real-time mocap mode, you need to delete the AnimatorController reference from Animator component on the model, and make sure that the networking parameters are the same as those set-up on the Perception Neuron software settings.

![img2](img/howto.png)
![img2](img/howto2.png)
