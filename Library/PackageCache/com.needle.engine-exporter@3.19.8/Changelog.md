# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [3.19.8] - 2023-10-10
### Unity Integration
- Add: Animation component export for clips array
- Bump Needle Engine version

### Needle Engine
- Add: `AnimatorController.createFromClips` utility method taking in a animationclips array to create a simple controller from. By default it creates transitions to the next clip
- Fix: occasional issue where the scrollbar would cause flickering due to hiding/showing when the website was zoomed
- Fix: screenshot utility method respecting page zoom
- Fix: vite dependency watcher plugin running installation if dependency in package.json would change
- Fix: Animator root motion working with multiple states, clips and transitions

## [3.19.7] - 2023-10-04
### Unity Integration
- Add: OrbitControls `enableRotate` property
- Change: LODGroup export in correct format and fix issue with last LOD not being used in cases without a "Cull" state

### Needle Engine
- Add: OrbitControls `enableRotate` property
- Fix: LODGroup not using last LOD in cases where the last LOD is never culled
- Fix: PostProcessing EffectStack correctly ordered when using N8 Ambient Occlusion (together with Bloom for example)
- Fix: Postprocessing N8 should not modify gamma if it's not the last effect in the stack
- Fix: AudioSource does now create an AudioListener on the main camera if none is found in the scene
- Change: VideoPlayer does fallback to clip if src is empty or null
- Change: OrbitControls now expose `enableRotate` property
- Fix: web component font import

## [3.19.4] - 2023-09-29
### Unity Integration
- Bump Needle Engine version

### Needle Engine
- Fix: Remove leftover OrbitControls log
- Change: Timeline TrackModel `markers` and `clips` fields are now optional
- Change: VideoPlayer is set to use url as default video source (if nothing is defined)

## [3.19.3] - 2023-09-28
### Unity Integration
- Bump UnityGLTF dependency to `2-4-2-exp` which fixes export for root level objects marked as `EditorOnly`

### Needle Engine
- Fix: regression in OrbitControls without lookat target assigned
- Fix: progressive textures loading with custom reflection probe
- Fix: WebAR touch event screenspace position using `this.context.input` 

## [3.19.2] - 2023-09-27
### Unity Integration
- Add: OrbitControls `autoFit` property
- Fix: Error when creating a FTPServerAsset in Unity 2022.3

### Needle Engine
- Add: OrbitControls `autoFit` property
- Add: API to access underlying Rapier physics body using `context.physics.engine.getBody(IComponent | IRigidbody)`

## [3.19.1] - 2023-09-27
### Unity Integration
- Add: ParticleSystem now supports HorizontalBillboard and VerticalBillboard
- Fix: exception in ComponentGenerator when clicking `regenerate components` in npmdef without ever having opened a Needle Project

### Needle Engine
- Add: ParticleSystem now supports HorizontalBillboard and VerticalBillboard
- Fix: [WebXR chromium bug](https://bugs.chromium.org/p/chromium/issues/detail?id=1475680) where the tracking transform matrix rotates roughly by 90° - we now add an WebXR Anchor to keep the scene at the placed location in the real world
- Fix: SceneSwitcher does now call event on first `ISceneEventListener` found on root level of a loaded scene (e.g. if a Unity scene is loaded that contains multiple children and does not have just one root object)
- Fix: Text UI clipping with multiple active screenspace canvases in scene
- Fix: Screenspace canvas events should not be blocked anymore by objects in 3D scene
- Fix: FirstPersonController rotation not being correctly / falsely resetted and flipped in some cases

## [3.19.0] - 2023-09-26
### Unity Integration
- Add: commandline argument to accept EULA via `--accept-needle-eula`
- Change: disable ExportInfo UI while cloning a remote project template
- Change: move react three fiber project template into remote repository

### Needle Engine
- Fix: collider scale wrongly affecting physics objects
- Fix: collider debug lines should not be raycastable
- Fix: mesh-collider behaving unexpectedly
- Fix: animator root motion causing error due to uninitialized Quaternion object

## [3.19.0-pre] - 2023-09-25
### Unity Integration
- Add: Project templates cloneable from github (added Sveltekit, Svelte and React templates)
- Fix: Improve installation of npmdef dependencies to be able to just click Play when opening or switching a sample
- Fix: saving of remote url in FTP server asset
- Fix: ShadowCatcher `Create` button creating plane with wrong rotation in some cases
- Change: clarify EULA window text
- Internal: Fix SampleInfo asset not being editable in inspector

## [3.18.0] - 2023-09-21
### Unity Integration
- Add: SceneSwitcher has now a field for `LoadingScene` which can be used to display a scene / 3D content while loading other scenes
- Fix: Improve license check
- Fix: Rare MissingReference error caused by EditorSync component while adding/removing components inside a prefab
- Change: Improve feedback when clicking the red Typescript component link (for scripts used in the scene but not installed in the web project)
- Change: Improse feedback for Needle Engine Pro Trial limits

### Needle Engine
- Add: SceneSwitcher has now a field for `loadingScene` which can be used to display a scene while loading other scenes
- Add: `ISceneEventListener` which is called by the SceneSwitcher when a scene has been loaded or a scene is being unloaded. It can be used to handle showing and hiding content gracefully. It has to be added to the root of the scene that is being loaded (e.g. the root of the scene or prefab assigned to the `loadingScene` field or the root of a scene assigned to the `scenes` array)
- Add: `hide-loading-overlay` attribute to `<needle-engine>` webcomponent (use like `<needle-engine hide-loading-overlay>`). Custom loading requires a PRO license. See [all attributes in the documentation](https://engine.needle.tools/docs/reference/needle-engine-attributes.html).
- Fix: Loading overlay should not link to needle website anymore when using a custom logo
- Fix: Add safeguard to user assigned events on `<needle-engine>` for cases where methods are not defined in the global scope
- Change: Update loading message displayed in overlay while waiting for `ContextCreated` promise (e.g. in cases where a large environment skybox is being loaded)

## [3.17.1] - 2023-09-20
### Unity Integration
- Fix: DeployToFTP deployment producing wrong meta image url 

## [3.17.0] - 2023-09-20
### Unity Integration
- Add: ExportInfo `remoteUrl` field which allows to pull or download projects from a remote repository instead being created from a local template
- Fix: ExportInfo shows an error if the directory paths contains invalid characters

### Needle Engine
- Fix: handle exception when loading GLB/glTF files with invalid lightmapping extension

## [3.16.5] - 2023-09-18
### Unity Integration
- Bump Needle Engine version

### Needle engine
- Add: help balloon message if user tries to open a local file without using a webserver
- Add: helpful console.log if user tries to add a component that is not a Needle Engine component
- Change: Ignore shadow catcher and GroundProjectedEnvironment sphere when running OrbitControls.fit

## [3.16.3] - 2023-09-15
### Unity Integration
- Add: `EditorModificationHandler.HandleChange` event to allow modification (or ignoring) of editor modifications, ignore UnityEvent changes by default
- Change: EditorSync ping should not run on main thread and block the editor

### Needle Engine
- Add: logo now respects prefer-reduced-motion, reduce and is immediately added instead of after 1s
- Fix: use default background color if GLB without camera and skybox is loaded
- Fix: ensure custom KTX2 loader is correctly initialized
- Fix: revert RectTransform change that broke hotspot rendering
- Change: adjust default backgroundBlurriness to match Blender defaults

## [3.16.2] - 2023-09-15
### Unity Integration
- Fix: License Check for exporting AnimatorController animation

### Needle Engine
- Add: mesh collider handling for invalid mesh data (non-indexed geometry)

## [3.16.2-pre] - 2023-09-13
### Needle Engine
- Add: `camera.environmentIntensity` property
- Change: default background blurriness for fallback camera to match blender default

## [3.16.1-pre] - 2023-09-13
### Needle Engine
- Change: if loaded glTF doesnt contain a camera we now also create the default OrbitControls (e.g. glTF exported from a Blender scene without a camera)

## [3.16.0-pre] - 2023-09-13
### Needle Engine
- Add: `NEEDLE_lightmaps` entries `pointer` property can now also be a path to a local texture on disc instead of a texture pointer. This allows Blender EXR and HDR maps to be used at runtime until Blender export supports hdr and exr images to be stored inside the GLB

## [3.15.0-pre] - 2023-09-13
### Unity Integration
- Fix: glTF `OnAfterImport` exception if imported glTF produced a missing GameObject

### Needle Engine
- Fix: remove leftover console.log
- Fix: `DeviceFlag` component not detecting devices correctly for iOS safari
- Fix: loading glTF without any nodes
- Fix: `SceneSwitcher` bug where a scene would be added twice when switching in fast succession
- Fix: `Animation.isPlaying` bool was always returning false
- Fix: Handle typescript 5 decorator change to prevent VSCode error message (or cases where `experimentalDecorators` is off in tsconfig). See [179](https://github.com/needle-tools/needle-engine-support/issues/179)
- Fix: Improve internal lifecycle checks and component method calls
- Change: Improse ContextRegistry/NeedleEngine `ContextEvent` enum documentation
- Change: `<needle-engine skybox-image=` and `environment-image=` attributes are now awaited (loading overlay is still being displayed until loading the images have finished or failed loading)

## [3.14.0-pre] - 2023-09-11
### Unity Integration
- Add: custom shader material inspectors now have UI with export options and information
- Remove: react template, please use https://github.com/needle-engine/react-sample instead
- Update: react-three-fiber template
- Bump UnityGLTF dependency to 2.4.1-exp containing an fix for KHR_animation_2 export and added compressed texture import

### Needle Engine
- Add: exposing `Needle.glTF.loadFromURL` in global scope to support loading of any glTF or GLB file with needle extensions and components when using the prebundled needle engine library (via CDN)
- Add: `context.update` method for cases where needle engine is now owning renderer/scene/camera and the update loop is managed externally
- Fix: animating custom shader property named `_Color` 
- Fix: issue with wrong CSS setting in Needle Engine breaking touch scroll
- Change: `?stats` now logs renderer statistics every few seconds
- Change: simplify creating a new Needle Context that is controlled externally (not owning camera/renderer)

## [3.13.0-pre] - 2023-09-08
### Unity Integration
- Add: `ActionsBrowser.BeforeOpen` event to allow modification to local server url or to customize the browser being opened / handle browser opening yourself
- Bump UnityGLTF dependency to 2.4.0-exp containing various color export fixes
- Fix: EditorSync issue when dragging transform position.x
- Fix: ArgumentOutOfRange exception in UnityEvent when no method name is assigned (or missing)
- Fix: Various cases where colors where exported in wrong colorspace affecting UI, materials and particles

### Needle Engine
- Add: ParticleSystem now also uses material color
- Add: `IEditorModificationListener.onAfterEditorModification` callback (requires `@needle-tools/editor-sync@2.0.1-beta`)
- Bump: Three.Quarks dependency to 0.10.6
- Update draco decoder include files

## [3.12.2-pre] - 2023-09-04
### Unity Integration
- Fix: Unity Progress display description on Windows interpreting `\n` as a newline which caused description to be cut off

### Needle Engine
- Add: option to override peerjs host and id (options) via `setPeerOptions`
- Fix: potential nullreference error in AudioListener
- Fix: Networking component cases where invalid localhost input with "/" causes url to contain "//" sometimes -> we can skip one "/" in this case and make it just work for users
- Fix: package.json `overrides` syntax for quarks three.js version
- Change: Screensharing bool to disable click to start networking + add deviceFilter to share(opts:ScreenCaptureOptions)

## [3.12.1-pre.1] - 2023-09-04
### Unity Integration
- Change: Update npmdef package version dependencies
- Fix: Handle `Win32 operation completed successfully` exception 

## [3.12.1-pre] - 2023-09-04
### Unity Integration
- Fix: editor web request failing on OSX

### Needle Engine
- Fix: next.js/webpack useRapier setting
- Change: typestore is now globally registered

## [3.12.0-pre.4] - 2023-08-29
### Unity Integration
- Change: Update cloning a project from github

## [3.12.0-pre.3] - 2023-08-28
### Unity Integration
- Change: Update git clone
- Fix: exporting all colors in linear colorspace now

## [3.12.0-pre.2] - 2023-08-28
### Unity Integration
- Fix: UnityEvent arguments not being used anymore

### Needle Engine
- Fix: vite hot reload plugin to dynamically import needle engine to support usage in server-side rendering context

## [3.12.0-pre] - 2023-08-28
### Unity Integration
- Add: commonly used skyboxes
- Change: Drop support for Unity 2020
- Change: bump UnityGltf from 2.2.0-exp to [2.3.1-exp](https://github.com/prefrontalcortex/UnityGLTF/blob/dev/CHANGELOG.md)
- Fix compiler error caused by HideInCallstacks in Unity 22.1.23
- Fix: issue where needle.config `assetDirectory` path wasn't respected (e.g. for sveltekit)

### Needle Engine
- Add: Timeline api for modifying final timeline position and rotation values (implement `ITimelineAnimationCallbacks` and call `director.registerAnimationCallback(this)`)
- Change: Update three quarks particlesystem library to latest
- Fix: issue where onPointerExit was being called when cursor stops moving
- Fix: USDZ normal scale Z was incorrect
- Fix: Timeline Signal events using different casing than UnityEvent events
- Fix: issue where `isLocalNetwork` was falsely determined

## [3.11.6] - 2023-08-15
- Remove beta

## [3.11.6-beta] - 2023-08-14
### Unity Integration
- Add: Tag filters to samples window
- Fix: HideInCallstacks compiler error

### Needle Engine
- Fix: find exported animation by PropertyBinding 
- Fix: USDZExporter was not exporting animation from Animation component but only from Animator
- Fix: potential issues with Animation component `clip.tracks` being null/undefined on USDZ export
- Fix: `loadstart` event not being called
- Fix: getComponent should always either the component or null (never undefined)
- Fix: dynamic import of websocket-ts package
- Fix: progressive texture loading wasn't properly awaited on USDZ export
- Fix: apply XR flags when exporting to QuickLook
- Fix: USDZ alpha clipping for materials without textures
- Fix: USDZ same material used in different ChangeMaterialOnClick resulted in duplicate behaviour names
- Change: set default WebARSessionRoot to "1" instead of "5"

## [3.11.5-beta] - 2023-08-10
### Unity Integration
- Fix: Shader export uniform parsing and error log
- Fix: Opening Typescript files in Visual Studio or Rider (Unity Default Code Editor) [issue 175](https://github.com/needle-tools/needle-engine-support/issues/175)
- Fix: issue with logging into file in certain cases on windows
- Fix: incorrect warning when wanting to clone from a repository that ends with `/` (via ExportInfo project path)
- Fix: extra styles in template and absolute positioning of shadowroot div
- Change: sanitize live url in DeployToFTP
- Change: add `.DS_Store` to gitignore
- Internal: add tracing scenario to WebHelper, explicit 1s timeout waiting for npm package response
- Internal: Bugreporter improvements

### Needle Engine
- Fix: components keep their gameObject references until `destroy` call of object's is completed when destroying an hierarchy. Previously child components might already be destroyed resulting in `myChildComponent.gameObject` being null when called in `onDestroy` from a parent component
- Fix: regression where timeline was not updating animations anymore if Animator had an empty AnimatorController assigned (without any states)
- Fix: `SceneSwitcher.switchScene` can now handle cases where it's called with a string url instead of an AssetReference 
- Fix: issue where `onPointerMove` event was being called continuously on mobile after touch had already ended
- Fix: issue where GLTFLoader extensions where missing name field resulting in extensions not being properly registered (causing stencils to not work sometimes)
- Change: EventSystem raycast is now only performed when pointer has changed (moved, pressed, down, up) which should improve performance on mobile devices when raycasting on skinned meshes
- Change: peer and websocket-ts import asynchronously
- Remove: legacy include files

## [3.11.4-beta] - 2023-08-04
### Unity Integration
- Bump UnityGLTF fixing issue with blend shape animation not being exported if animation also contained humanoid animations

### Needle Engine
- Fix: USDZExporter exception caused by programmatically calling `exportAsync` without quicklook button
- Fix: Timeline `evaluate` while being paused
- Bump three to fix issue with blend shape animation not being applied to Group objects (KHR_animation_pointer)

## [3.11.3-beta] - 2023-08-03
### Unity Integration
- Add: new option to export glb from context menu without progressive texture processing
- Change: Improve feedback when installing samples
- Fix: finding toktx default installation on MacOS

### Needle Engine
- Change: improve styling of `<needle-engine>` DOM overlay element to allow positioning of child elements
- Fix: USDZExporter normal bias when normalScale is used
- Fix: Nullreference in SceneSwitcher when creating the component from code and calling `select` with a new scene url
- Fix: Quicklook button creation
- Fix: Particlesystem layermask not being respected

## [3.11.2-beta] - 2023-07-31
### Unity Integration
- Fix: CustomReflection texture should not be renamed
- Fix: CustomReflection texture should be at least 64 pixel when exporting
- Change: Bump UnityGLTF dependency, fixing texture transform export for metallicRoughness
- Change: improve "Setup Scene" default names

### Needle Engine
- Fix: `ChangeMaterialOnClick` with multi material objects
- Fix: progressive textures regression

## [3.11.1-beta] - 2023-07-31
### Unity Integration
- Minor editor UI changes

### Needle Engine
- Add: `saveImage` utility method and make `screenshot` parameter optional
- Add: `loading-style="auto"`
- Fix: skybox image caching
- Fix: finding animation tracks for unnamed nodes when using the `autoplay` attribute
- Change: improved `<needle-engine>` default sizes
- Change: smoother src changes on `<needle-engine>` by only showing loading overlay when loading of files takes longer than a second
- Change: bump three version to 154.2 fixing KHR_animation_pointer not working with SkinnedMesh

## [3.11.0-beta] - 2023-07-29
### Unity Integration
- Fix: hide FTP password in console logs
- Fix: incorrect check in Samples Window for installing samples in 2022 LTS and later 2023 LTS
- Change: show installed versions in ExportInfo even if web project is not yet installed

### Needle Engine
- Add: Support for blending between Timeline and Animator animation by fading out animation clips allowing to blend idle and animator timeline animations
- Fix: WebXR buttons style to stay inside `<needle-engine>` web component
- Fix: `OrbitControls.fitCamera` now sets rotation target to the center of the bounding box
- Fix: Timeline animation regression causing Animator not being enabled again after timeline has finished playing
- Fix: Timeline should re-enable animator when ended reached end with wrap-mode set to None
- Change: add `.js` extensions to all imports
- Change: allow overriding loading style in local develoopment
- Change: expose flatbuffer scheme helper methods

## [3.10.7-beta] - 2023-07-28
### Unity Integration
- Fix: Shader uniform export
- Fix: edge case when using URLs in ExportInfo directory 
- Fix: console log prints in certain cases containing control characters
- Fix: Toktx detection not working properly on OSX
- Change: Warn if debug mode is enabled

### Needle Engine
- Fix: Camera using RenderTarget (RenderTexture) now applies clear flags before rendering (to render with solid color or another skybox for example)
- Fix: RenderTexture not working in production build due to texture being compressed
- Fix: RenderTexture warning `Feedback loop formed between Framebuffer and active Texture`
- Fix: Handle Subparticlesystem not being properly serialized avoiding runtime error
- Internal: add resource usage tracking of textures and materials

## [3.10.6-beta] - 2023-07-27
### Unity Integration
- Bump Engine version

### Web Engine
- Fix: Timeline ActivationTrack behaves like `leave as is` when timeline is disabled (not changing the activate state anymore)
- Fix: Timeline Signal Track with duration of 0 and signal at time 0 does now trigger
- Fix: Timeline disabling or pausing does now activate animator again
- Fix: CustomShader Time node for BiRP
- Fix: ParticleSystem simulation mode local now correctly applies parent scale
- Change: Show warning for wrong usage of `@serializable` with `Object3D` where a `AssetReference` is expected
- Change: ParticleSystem shows warning when using unsupported scale mode (we only support local right now)

## [3.10.5-beta] - 2023-07-25
### Unity Integration
- Add: DeployToFTP does not support SFTP
- Add: `overscroll-behaviour` CSS to templates
- Add: `type: module` to templates
- Fix: issue where ftp deployment didnt work on OSX
- Fix: export of referenced scenes or prefabs with timeline where timeline graph was exported in the wrong state

### Web Engine
- Fix: warning at runtime when methods called by `EventList`/`UnityEvent` are in the wrong format
- Fix: OrbitControls issue where double clicking/focus on screenspace UI would cause the camera to be moved far away
- Fix: `OrbitControls.fitCamera` where three `expandByObject` now requires an additional call to `updateWorldMatrix` [26485](https://github.com/mrdoob/three.js/issues/26485#issuecomment-1649596717)
- Change: replace some old `Texture.encoding` calls with new `Texture.colorSpace`
- Change: improve `PlayerSync` networking and add `onPlayerSpawned` event
- Remove: `RectTransform.offsetMin` and `offsetMax` because it's not implemented at the moment

## [3.10.4-beta] - 2023-07-24
### Unity Integration
- Fix: rare InvalidOperationException when codewatcher list is cleared while foreach runs

### Web Engine
- Fix: activating UI elements in VR not applying transform

## [3.10.3-beta] - 2023-07-21
### Unity Integration
- Internal: include package and Unity versions in bug report description

### Web Engine
- Fix: AnimatorController error caused by missing animationclip
- Fix: next.js webpack versions plugin
- Fix: Occasional `failed to load glb` error caused by not properly registering `KHR_animation_pointer` extension 
- Fix: UI issue where Text in worldspace canvas would be visible at wrong position for a frame
- Fix: UI issue where Text would not properly update when switching between text with and without richtext
- Fix: UI issue where Image would not automatically update when setting texture from script
- Fix: issue where RenderTexture would not be cleared before rendering
- Change: make `addEventListener` attribute on `<needle-engine>` optional

## [3.10.2-beta] - 2023-07-19
### Unity Integration
- Change: Samples window clicking `Install Samples` now displays feedback that samples are being installed

### Web Engine
- Fix: iOS double touch / input
- Change: minor WebXRController refactor moving functionality into separate methods to be patchable

## [3.10.1-beta] - 2023-07-18
### Unity Integration
- Fix: workaround for TextureImporter.spritesheet being obsolete without proper replacement
- Change: update LTS version warning; no warning on 2022 LTS (and 2023 LTS) but warn on 2020 LTS since that's out of support.

### Web Engine
- Fix: prebundled package
- Fix: runtime license check
- Fix: Input being ignored after first touch
- Fix: SpatialTrigger, reverting previous change where we removed the trigger arguments

## [3.10.0-beta] - 2023-07-17
### Unity Integration
- Fix: shadow catcher BiRP support
- Change: add link to [feedback form](https://fwd.needle.tools/needle-engine/feedback) to License window

### Web Engine
- Fix: Text clipping in VR
- Fix: AR overlay `quit-ar` button not being properly detected
- Fix: Timeline animation track post-extrapolate set to `Hold`
- Fix: iOS touch event always producing double click due to not properly ignoring mouse-up event
- Change: DragControls to automatically add ObjectRaycaster if none is found in parent hierarchy
- Change: DragControls now expose options to hide gizmo and to disable view-dependant behaviour

## [3.10.0-exp] - 2023-07-15
### Exporter
- Add: support to download project via git repository
- Fix: issue with opening project directory for certain relative paths

### Engine
- Change: WebXR component now automatically adds a WebARSessionRoot on entering AR when no session root was found in the scene
- Change: `@syncField` can now sync objects by re-assigning the object to the same field (e.g. `this.mySyncedObject = this.mySyncedObject` to trigger syncing)
- Change: log error when `@syncField` is being used in unsupported types (currently we only support syncField being used inside Components directly)
- Change: improve message when circular scene loading is detected and link to documentation page 

## [3.9.1-exp] - 2023-07-14
### Exporter
- Fix: compiler errors in Unity 2023.1
- Fix: bug in npmdef registry causing packages to not be properly registered on first editor startup
- Fix: OSX editor stall due to FileWatcher
- Change: Add badge to scene templates
- Change: Don't make insecure calls (localhost running on `http`) when `PlayerSettings.insecureHttpOption` is turned off starting from Unity 2022
- Change: component compiler should ignore .d.ts files
- Change: component compiler can now work without web project (only requires ExportInfo in the scene)
- Internal: cleanup Collab Sandbox scene template, remove unused material

### Engine
- Add: SceneSwitcher now uses scene name by default. Can be turned off in component
- Fix: ParticleSystem lifetime not respecting simulation speed 
- Fix: ParticleSystem prewarm with simulation speed and improved calculation of how many frames to simulate
- Fix: Exit AR and Exit VR now restores previous field of view
- Change: close AR button adjusted for better visibility on bright backgrounds
- Change: bump @types/three to 154

## [3.9.0-exp] - 2023-07-12
### Exporter
- Bump Engine version

### Engine
- Add: `<needle-engine>` web component slot support, AR DOM overlay can now be added by simple adding HTML elements inside the `<needle-engine></needle-engine>` web component. Fixing [165](https://github.com/needle-tools/needle-engine-support/issues/164)
- Add: Basic USDZ exporting of UI shadow hierarchies as mesh hierarchies for UI in Quicklook AR support
- Fix: WebXR Rig not being rotated as expected when setting up in Unity [129](https://github.com/needle-tools/needle-engine-support/issues/129)
- Fix: WebXR VR button click, hover is still not working
- Fix: Issue with Lightmaps breaking when switching back and forth between multiple lightmapped scenes
- Change: Button click should not invoke with middle or right mouse

## [3.8.0-exp] - 2023-07-11
### Exporter
- Bump Engine version to use three.js 154 (latest)

### Engine
- Update three.js to 154 (latest)
- Bump postprocessing dependency
- Add: `this.context.xrCamera` property
- Fix: screenspace canvas should not run in VR
- Fix: OrbitControls should not update while in AR and touching the screen
- Change: allow using vanilla three.js by dynamically importing KHR_animation pointer api 

## [3.7.7-pre] - 2023-07-11
### Exporter
- Bump Engine

### Engine
- Fix: LookAt copyTarget + keepUpDirection
- Fix: DragControls not working on first touch on mobile / clone input event
- Fix: Renderer assigning renderOrder in URP on SkinnedMesh with multi-material

## [3.7.6-pre] - 2023-07-10
### Exporter
- Fix: react and r3f templates
- Fix: warnings on OSX
- Fix: invalid cast exception due to change with prefab export
- Change: use UnityWebRequest.EscapeURL for BugReporter description
- Change: show reason for why Bugreporter dialogue popup shows again

## [3.7.5-pre.3] - 2023-07-08
### Exporter
- Fix: compiler error in Unity 2021.3.28 (latest)

## [3.7.5-pre.2] - 2023-07-07
### Exporter
- Fix: Unity 2020.3 compiler error

## [3.7.5-pre.1] - 2023-07-07
### Exporter
- Fix: minor 2021+ compiler warning
- Change: Allow longer bug report descriptions

## [3.7.5-pre] - 2023-07-07
### Exporter
- Add: support for bugreporter descriptions
- Change: Fonts handle semibold variant
- Change: make sure PlayerSync can be enabled/disabled in the editor
- Internal: specifically log when reading file is not allowed

### Engine
- Fix: USDZExporter should not show Needle banner when branding information is empty (pro only)
- Fix: USDZExporter sessionroot scale should be applied to object to be exported when the root is in the parent
- Fix: DropListener localhost without explicit backend url + dropping file caused exceptions
- Fix: instanceof error that tsc complained about
- Change: Fonts handle semibold variant
- Internal: DropListener re-use addFiles method, remove old code
- Internal: Bump tools package dependency

## [3.7.5-exp] - 2023-07-06
### Exporter
- Fix: catch access lock exception when trying to read npm log
- Change: component in prefab referencing root prefab should not export as glb path

### Engine
- Add: SignalEvents support for arguments
- Fix: SpatialTrigger Unity events removing extra (unexpected) event arguments
- Fix: safeguard `AudioSource.play` to not fail when `clip` argument is not a string
- Change: change Timeline signal event trigger time to use last frame deltatime with padding to estimate if the event should fire

## [3.7.4-exp] - 2023-07-05
### Exporter
- Fix: Sprite colorspace export taking sRGB textures into account

### Engine
- Change: targetFps, use timestamp that we get from the animation callback event because it is more reliable on Firefox

## [3.7.3-exp] - 2023-06-26
### Exporter
- Bump engine version

### Engine
- Add: physics gravity to `IPhysicsEngine` interface to be available via `this.context.physics.engine.gravity`
- Fix: USDZ text alignment

## [3.7.2-exp] - 2023-06-23
### Exporter
- Add: `Preview Build` button to PlayerBuildWindow
- Fix: PlayerBuildWindow for Unity 2022.3.3 

### Engine
- Fix: Nullref in SpectatorCamera.onDestroy when camera wasnt active

## [3.7.1-exp] - 2023-06-22
### Exporter
- Fix: Font export with styles that are unknown to the Unity FontStyle enum (e.g. `-Medium`)

### Engine
- Add: ChangeMaterialOnClick `fadeDuration` option (Quicklook only)
- Change: USDZ export now enforces progressive textures to be loaded before export
- Change: USDZ export callbacks for `beforeCreateDocument` and `afterCreateDocument` can now run async
- Fix: USDZExporter quicklook button
- Fix: USDZExporter Quicklook button not being removed when exporter gets removed or disabled
- Fix: USDZ ChangeMaterialOnClick clear cache before exporting, this caused USDZ export to fail on third export in USDZ sample scene
- Fix: Engine loading bar not being updated
- Fix: USDZ text linebreaks
- Fix: UI font name style check. Unknown font styles are now not touched anymore (e.g. font name ending with `-Medium`)

## [3.7.0-exp] - 2023-06-21
### Exporter
- Bump Engine version

### Engine
- Change: Move HTML elements into <needle-engine> shadow dom

## [3.6.13] - 2023-06-21
### Exporter
- Fix: use assemblylock to handle regenerating all components in npmdef
- Bump Engine version

### Engine
- Add: static Context.DefaultTargetFrameRate
- Add: option to prevent USDZExporter from creating the button
- Fix: `@prefix` handling promise resolving to false

## [3.6.11] - 2023-06-19
### Exporter
- Bump UnityGLTF version adding support for importing draco compressed meshes and KTX2 compressed textures

### Engine
- Add: UI InputField API for clear, select and deselect from code
- Change: LODGroup serialization
- Fix: mobile debug console should be above loading overlay if error happens during loading
- Fix: LODGroup not being able to resolve renderer reference
- Fix: Particles direction being wrong in some causes with scaled parent and scaled particle system
- Fix: Particles subsystem emitter position being wrong when main particle system was scaled
- Fix: Bundled library failing to load due to undeclared variable
- Fix: UI InputField hide html element
- Fix: Joining empty room name is not allowed anymore
- Fix: Clamp Room name length to 1024 chars

## [3.6.10] - 2023-06-14
### Exporter
- Bump engine version

### Engine
- Fix: Text with richText not updating properly
- Internal: Change font style parsing

## [3.6.9] - 2023-06-12
### Exporter
- Bump Engine version

### Engine
- Fix: Particles SizeOverLifetime module for mesh particles

## [3.6.8] - 2023-06-12
### Exporter
- Fix: LookAt component exception when being used in prefab

## [3.6.6] - 2023-06-12
### Exporter
- Bump Engine version

### Engine
- Internal updates

## [3.6.5] - 2023-06-09
### Exporter
- Bump Engine version

### Engine
- Add: NestedGltf `loaded` event being raised when the glb has been loaded
- Add: AnimationCurve cubic interpolation support
- Change: set targetFramerate to 60 by default (in `context.targetFrameRate`)
- Fix: USDZ metalness/roughness potentially being undefined when exporting Unlit materials
- Fix: Handle exception when loading components due to bug when using meshopt compression and material count changes
- Fix: ColorAdjustments setting tonemapping exposure to 0 when exposure parameter override is off [824]

## [3.6.4] - 2023-06-02
### Exporter
- Bump engine version

### Engine
- Add: `ObjectUtils.createPrimitive` for cube and sphere
- Change: expose `ObjectUtils`
- Fix: BoxGizmo component
- Fix: vite copy plugin when needle.config.json "assets" directory starts with "/"

## [3.6.3] - 2023-06-01
### Engine
- Change: OrbitControls apply settings in update
- Fix: Rapier stripping not being respected

## [3.6.2] - 2023-06-01
### Exporter
- Change: Try fix curl 60 error when server is already running on http

### Engine
- Fix: wrong UI z-offset in some cases
- Fix: Particle velocity over lifetime not using world rotation
- Fix: Particle burst being played twice
- Fix: Particle `playOnAwake` option not being respected

## [3.6.2-pre] - 2023-05-31
### Exporter
- Bump Engine Version

### Engine
- Add: `setAllowOverlayMessages` to explictly disable overlay messages without url parameter
- Add: allow larger textures for USDZ generation
- Fix: nested gltf with disposing of resources leading to broken files

## [3.6.1-pre] - 2023-05-29
### Exporter
- Change: enable ProgressiveTexture compression by default. Use the `ProgressiveTextureSettings` component to explictly disable it

### Engine
- Fix: removing `<needle-engine>` from DOM does now dispose the context properly and unsubscribes from browser events. Add `keep-alive` attribute to disable disposing

## [3.6.0-pre] - 2023-05-29
### Exporter
- Add: `ScreenSpaceAmbientOcclusionN8` component
- Bump Engine version

### Engine
- Add callbacks for ContextClearing
- Add: [n8AO postprocessing effect](https://github.com/N8python/n8ao) (Screenspace Ambient Occlusion) support
- Add: option to disable automatic poster generation (use `noPoster` in options in vite.config) 
- Fix: `<needle-engine>` without any src should setup an empty scene
- Change: `OrbitControls.fit` now handles empty scene and ignores GridHelper
- Change: TimelineAudio disconnect audio in onDestroy
- Change: Ensure PostProcessing VolumeParameters are initialized
- Change: Improve memory allocs and disposing of resources
- Change: Update three.js fixing GLTFLoader memory leak

## [3.6.0-exp] - 2023-05-27
### Exporter
- Fix: Exception when npmdef package had no `devDependencies` key
- Bump Engine version

### Engine
- Add: Changing `src` attribute now does scene cleanup and loads new files
- Add: `skybox-image` and `environment-image` attributes, allow changing both at runtime 
- Fix: error display overlapping in cases where somehow engine is imported twice
- Fix: logo overlay should only show when loading is done, change error during render loop message
- Fix: OrbitControls camera fitting now done once before rendering when loaded glb does not contain any camera
- Fix: Vite client plugin imports
- Change: Context now handle errors during initializing or when starting render loop
- Change: ContextRegistry exported as NeedleEngine and export hasIndieLicense function
- Change: Remove need to manually define global engine variables in cases without bundler or Needle plugins

## [3.5.13-pre] - 2023-05-26
### Exporter
- Change: ExportInfo editor performance improvements: check if npm is installed only once per session, run project validation on thread, dont collect template files in onEnable
- Fix: Prevent spawning more than one "npm installed" check task

### Engine
- Change: OrbitControls camera fitting improved

## [3.5.12-pre] - 2023-05-24
### Exporter
- Add: `IAdditionalFontCharacters` interface to allow components to add additional characters for font atlas generation
- Change: schedule Font export task to be awaited at end of export
- Change: GltfValueResolver should export Object3D node reference instead of Transform if referencing GameObject in UI hierarchy

### Engine
- Add: option to toggle collider visibility from script via `this.context.physics.engine.debugRenderColliders` 
- Change: engine.physics raycast doesnt need any parameters now anymore
- Change: OrbitControls default target should be related to distance to center (if nothing is hit by raycast)
- Fix: EventList object and component argument deserialization

## [3.5.11-pre.1] - 2023-05-22
### Exporter
- Fix: missing texture for importer overrides inspector header

## [3.5.11-pre] - 2023-05-22
### Exporter
- Bump Engine Version

### Engine
- Add: `@registerType` decorator that can be added to classes for registration in TypeStore. Currently only useful for cases outside of Unity or Blender for Hot Reload support
- Fix: `Component.name` should return Object3D name
- Fix: GameObject static methods generic
- Fix: Logo animation causing browser scrollbar to appear

## [3.5.10-pre] - 2023-05-22
### Exporter
- Add: SpriteRenderer now exposes shadow casting and transparency options via SpriteRendererData component

### Engine
- Add: SpriteRenderer now exposes shadow casting and transparency options
- Fix: vite plugin issue caused by missing src/generated/meta
- Fix: nullref in debug_overlay, typo in physics comment
- Fix: disabling collider with rigidbody component did cause an error in rapier
- Fix: HTMLElement css, cleanup loading element, move logo into html element
- Fix: GameObject.addComponent now takes Object3D type too
- Fix: loading overlay not hiding when <needle-engine> src changes
- Change: OrbitControls now sets target to 10 meter by default if nothing is assigned or hit in the scene (previously it was set to 1 meter)
- Change: fit camera to scene after loading when no camera is present in file

## [3.5.9-pre.2] - 2023-05-20
### Exporter
- Fix: Component links should use default app

### Engine
- Add: WebXRPlaneTracking should initiate room setup on Quest when no tracked planes are found

## [3.5.9-pre.1] - 2023-05-19
### Exporter
- Fix: DeployToGlitch
- Internal: move compression components into Needle AddComponent sub-menu

### Engine
- Fix: SceneSwitcher should ignore swipe events when `useSwipe` is disabled

## [3.5.9-pre] - 2023-05-19
### Exporter
- Change: when using a custom reflection texture use the texture size
- Fix: issue where npmdef to react-three-fiber package was being removed when generating the project

### Engine
- Add: Support for progressive texture loading for custom shaders
- Fix: react-three-fiber template

## [3.5.9-exp.2] - 2023-05-18
### Exporter
- Bump Needle Engine package

### Engine
- Add: needle-engine attributes documentation
- Change: assign main camera during gltf component deserialization when no camera is currently assigned

## [3.5.9-exp] - 2023-05-18
### Exporter
- Change: allow opening component links with default editor too (when VSCode is unticked in Needle settings)
- Change: clear .next/cache directory on full export

### Engine
- Add: add nextjs plugin to handle transpiling and defines
- Change: expose USD types to make custom behaviours, add proximityToCameraTrigger
- Fix: loading element position to absolute to avoid jumps when added to e.g. nextjs template
- Fix: texcoords werren't quicklook compatible in ThreeUSDZExporter
- Fix: `LookAt` component with invertForward option was flipped vertically in QuickLook

## [3.5.8-exp] - 2023-05-16
### Exporter
- Add option to settings to open web projects and files with default code editor (e.g. Rider)
- Add NeedleConfig `baseUrl` for codegen e.g. when the served file path is not the local path (e.g. `./public/assets` but server url is `./assets`)
- Change: improve check for http and https, remove usage of UnityWebRequest because is logs ssl error that we can not prevent when pinging local server urls
- Change: dont append toktx path as argument anymore when running build command, it is automatically discovered by build pipeline
- Fix: NullReferenceException in ProjectGenerator

### Engine
- Add NeedleConfig `baseUrl` for codegen
- Change: AudioSource should pause in background on mobile
- Fix: logo svg import for nextjs
- Fix: particle system playOnAwake

## [3.5.7-exp] - 2023-05-15
### Exporter
- Add: Initial support for text in USDZ
- Fix: EditorSync, prevent error caused by serialization of UnityObject
- Fix: Components can now reference RectTransforms
- Change: expose `SyncedRoom.tryJoinRoom` method
- Change: add some more information to networking components
- Bump UnityGLTF fixing issues with material animation export

### Engine
- Add: Initial support for text in USDZ
- Change: add generic to `networking.send` for validation of model
- Change: SyncedRoom, expose tryJoinRoom method + remove error thrown when roomName.length <= 0, join room in onEnable
- Fix: setting position on UI object (RectTransform) works again
- Fix addressable instantiate options called with `{ position: .... }` and without a parent, it should then still take the scene as the default parent
- Fix: WebXR `arEnabled` option
- Fix: Worldspace canvas always being rendered on top
- Fix: CanvasGroup alpha not being applied to text

## [3.5.6-exp] - 2023-05-12
### Exporter
- Add component tags for easier searching of Everywhere Actions (USDZ/QuickLook support)

### Engine
- Add: `addComponent` method to this.gameObject
- Add: "light" version on bundle processing
- Add: bundled library now comes with `light` variant to be installed from cdn (e.g. [`needle-engine.light.min.js`](https://unpkg.com/@needle-tools/engine@3.5.6-alpha/dist/needle-engine.light.min.js))
- Remove: some spurious logs
- Fix: defines for vanilla JS usage
- Fix: CanvasGroup not overriding alpha

## [3.5.5-exp] - 2023-05-11
### Exporter
- Change license display: holt ALT to show clear text + trim whitespace
- Bump engine version

### Engine
- Add: getWorldDirection
- Add: needle.config.json `build.copy = []` to copy files on build from arbitrary locations into the dist folder for example:
  ```md
  "build": {
    "copy": [
      "cards" <-- can be relative or absolute folder or file. In this case the folder is named "cards" in the web project directory
    ]
  }
  ```
- Add ip and location utils
- Change: add buffers for getWorldQuaternion, getWorldScale util methods
- Change: animatorcontroller should only log "no-action" warning when in debug mode
- Fix: apply and check license

## [3.5.4-exp] - 2023-05-11
### Exporter
- Change: introduce FileReference and derive ImageReference from it, add FileReferenceTypeAttribute. It can be used to copy any file type from Unity to the desired output directory without modification or going through the exporter to, for example, reference `usdz` files.

### Engine
- Fix: wrong serialization check if a property is writeable
- Fix: mark UI dirty when text changes
- Change: allow UI graphic texture to be set to null to remove any texture/image
- Change: rename USDZExporter `overlay` to `branding`

## [3.5.3-exp.1] - 2023-05-10
### Engine
- Fix: wrong check in serialization causing particles to break (introduced in 3.5.3-exp)

## [3.5.3-exp] - 2023-05-10
### Exporter
- Change: hold ALT to show Netlify access key

### Engine
- Add: `IPointerMoveHandler` interface providing `onPointerMove` event while hovered
- Add: USDZ AudioSource support and PlayAudioOnClick
- Change: balloon messages can now loop
- Change: pointer event methods are now lowercase
- Change: allow `moveComponent` to be called with component instance that was not added to a gameObject before (e.g. created in global scope and not using the `addComponent` methods)
- Fix: input pointer position delta when browser console is open
- Fix: GameObject.destroy nullcheck
- Fix: typescript error because of import.meta.env acccess
- Fix: issue where added scenelighting component by extension caused animation binding to break
- Fix: UI layout adding objects dynamically by setting anchorMinMax
- Fix: Prevent exception during de-serialization when implictly assigning value to setter property
- Fix: screenspace canvas being rendered twice when using explicit additional canvas data component
- Fix: EventSystem cached state of hovered canvasgroup not being reset causing no element to receive any input anymore after having hovered a non-interactable canvasgroup once
- Fix: empty array being returned in `GameObject.getComponents` call when the passed in object was null or undefined

## [3.5.2-exp] - 2023-05-09
### Exporter
- Add: SceneSwitcher preload feature
- Change: USDZBehaviours can now be enabled on USDZExporter component

### Engine
- Add: SceneSwitcher preload feature
- Change: interactive behaviours for QuickLook are on by default now
- Fix: SetActiveOnClick toggle for QuickLook
- Fix: USDZ texture transform export works in more cases

## [3.5.1-exp] - 2023-05-09
### Exporter
- Change: Allow overriding the default GltfValueResolver

### Engine
- Fix: reflection probes not working anymore
- Fix: false RectTransform return breaking some cases with reparenting
- Fix: RectTransform mark dirty when anchors change (due to animation for example)

## [3.5.0-exp] - 2023-05-08
### ⭐ Highlights
#### **Tree-shake Rapier / Physics engine**
With this version the physics engine can be marked to be removed in bundles reducing the overall `needle-engine` size by 600 KB (when using gzip) or 2 MB (without gzip). See the [documentation](https://fwd.needle.tools/needle-engine/docs/compression) for more information

#### **Choose between draco and meshopt mesh compression**
Add support to compress exported glTFs either with draco or meshopt compression. See the [documentation](https://fwd.needle.tools/needle-engine/docs/compression) for more information

#### **Various USDZ export fixes**
This release fixes various issues with USDZ export like exporting occlusion maps, texture input scale not being used and normal maps color space

### Exporter
- Add: vite plugin to watch package.json dependency changes to restart the server (can be disabled by adding `{noDependencyWatcher:true}` as a third parameter to the needle plugin)
- Add: `MeshCompression` component to be able to select compression per prefab/scene/gltfobject
- Add: `NeedleEngineModules` component to be able to remove rapier from bundle reducing overall engine size by 2MB (or 600KB with gzipping)
- Fix: nullref when adding new `DeployToFTP` component
- Fix: colorspace and texture flip issues in USDZ export in production builds (compressed texture readback)

### Engine
- Change: allow tree-shaking rapier physics
- Fix various USDZ export issues:
  - fix UV2 for occlusion maps (paves the way for lightmaps), had to be texCoord2f[] instead of float2[]
  - fix missing MaterialBindingAPI schema
  - fix normal scale for non-ARQL systems (ARQL doesn't support it though, but needed for other viewers)
  - fix input:scale for textures not being used if it's (1,1,1,1)
  - fix normal maps not being in raw colorSpace

## [3.4.0-exp.1] - 2023-05-05
### Exporter
- Fix: inspector injections stopped working

## [3.4.0-exp] - 2023-05-05
### ⭐ Highlights
#### **QuickLook Behaviours (experimental)**
This version adds support for interactive USDZ files for iOS devices. A number of built-in components work out of the box, with more to come! Try the USDZExporter sample to see for yourself. The high-level components will likely change over the next releases, but now is a great time to experiment and provide feedback.  

#### **AR Image Tracking**
AR Image Tracking is now available! Place content on trackable, configurable markers. On Android, it requires Chrome and currently the flag `chrome://flags#webxr-incubations` needs to be enabled. On iOS, Image Tracking works without additional settings.

#### **UI Improvements**
This version adds initial support for Vertical- and Horizontal LayoutGroups for Unity's UI Canvas System.

### Exporter
- Add: high-level USDBehaviours components: ChangeMaterialOnClick, PlayAnimationOnClick, SetActiveOnClick, HideOnStart
- Add: DeployToFTP: add option to disallow toplevel deployment
- Add: SamplesWindow filtering by tags and sorting by priority
- Change: various editor performance improvements
- Change: add @types/three when generating new NpmDefs
- Bump UnityGLTF dependency including fixes for NaNs in Unity's tangents and sorting of AnimationClip channels

### Engine
- Add: low-level USD Actions/Triggers API for building complex interactions for iOS devices
- Add: high-level USDBehaviours components: ChangeMaterialOnClick, PlayAnimationOnClick, SetActiveOnClick, HideOnStart
- Add: LookAt component now supports iOS AR
- Add: more settings for LookAt
- Add: support for Horizontal- and VerticalLayoutGroup (UI)
- Fix: `setWorldScale` was setting incorrect scale in some cases
- Fix: WebXR Image Tracking now works with WebARSessionRoot / rig movements
- Fix: vite reload only when files inside "assets" change and only if its a known file type
- Fix: UI scale set to 0 not being applied correctly

## [3.3.0-exp] - 2023-05-02
### ⭐ Highlights 
#### **Screenspace UI and improved RectTransform support**  
This versions updates to latest three-mesh-ui 7.x and adds support to correctly apply RectTransform anchoring and pivot settings as well as the ability to create screenspace UI (both modes for screenspace overlay and screenspace camera are supported)

### Exporter
- Add: deploy to github pages component
- Add: Linked npm package support
- Fix: recursively installing locally referenced packages
- Fix: check if scene is saved before trying to export when not using any GltfObject
- Fix Unity warning when exporting canvas in scene without GltfObject
- Change: SceneSwitcher now allows assigning both prefabs and scenes

### Engine
- Add: AssetReference can now handle scene reference
- Add: UI update with support for screenspace UI, anchoring, pivots, image outline effect, image pixelPerUnit multiplier
- Add: basic LookAt component
- Add: basic UI outline support + handle builtin Knob image
- Add: WebXRImageTracking ability to directly add a tracked object to an image marker
- Fix: OrbitControls should only update when being the active camera
- Fix: UI input ignored browser "mouseDown" for each "touchUp" event
- Fix: OrbitControls requiring additional tab after having clicked on UI
- Fix: OrbitControls only being deactivated when down event starts on UI element
- Fix: loading bar text not being decoded (displayed e.g. `%20` for a space)
- Fix: TransformGizmo not working anymore

## [3.2.15-exp] - 2023-04-28
### Exporter
- Add: USDZExporter exposes download usdz file name

### Engine
- Add: SceneSwitcher.select(AssetReference) support to be invoked from a UnityEvent with an object reference (must be an asset)
- USDZExporter: change exported usdz name, remove needle name for license holders

## [3.2.14-exp] - 2023-04-28
### Exporter
- Add: OpenURL component
- Fix: USDZ export breaking if the object name is just a number 
- Fix: allow to specify local three version in package

### Engine
- Add: OpenURL component
- Change: Implictly add Raycaster to scene if it is not found.
- Fix: USDZ export breaking if the object name is just a number 

## [3.2.13-exp.1] - 2023-04-27
### Exporter
- Fix: Vite template missing `base: "./"` for FTP subfolder deployment
- Fix: Vite template missing `server.proxy` option for HTTP2
- Change: DeployToFTP can now run `Build & Deploy` even if the project was never built before

## [3.2.13-exp] - 2023-04-27
### Exporter
- Add: USDZExporter editor shows warning if no objects are assigned and exposes quicklook overlay texts
- Add: USDZExporter callToActionButton can now invoke url to open
- Change: EditorSync improved feedback during installation
- Change: Remove Copy files run from editor, run copy files on via vite plugin
- Change: remove console log in pro license
- Fix: Fix vite html transform plugin
- Fix: EditorSync false check if Materials were enabled, otherwise it would not inject
- Fix: minor SemVer warning

### Engine
- Add: USDZExporter editor shows warning if no objects are assigned and exposes quicklook overlay texts
- Add: USDZExporter callToActionButton can now invoke url to open
- Add: SceneSwitcher can now use history without updating the url parameter
- Fix: Fix vite html transform plugin

## [3.2.12-exp] - 2023-04-26
### Exporter
- Change: ProcessHelper should fail if working directory doesnt exist
- Change: ProcessHelper starts command windows minimized
- Change: BugReporter can now run without web project
- Fix: BugReporter should run by using Needle managed tools package
- Fix: When mesh compression `override` was enabled the `useSimplifier` would not be used

### Engine
- Fix: issue where removing an object from the scene would disable all its components

## [3.2.11-exp] - 2023-04-25
### Exporter
- Bump Needle Engine version
- Bump Tools package version

### Engine
- Fix: lighting settings being implictly switched (enabled/disabled) when using SyncCamera / any loaded prefab at runtime

## [3.2.10-exp] - 2023-04-25
### Exporter
- Remove: creation of legacy `scripts.js` file
- Change: improve first time installation logs
- Change: Clean install now recursively runs for locally referenced packages
- Change: EditorSync now can allow camera sync only / only inject materials if enabled and only inject other properties if `components` sync is enabled
- Change: EditorSync should disable scene camera sync when a scene is closed to not lock camera view in browser
- Change: EditorSync: schedule reconnect exponentially slower over time if it fails

### Engine
- Fix: Remove log in `Animator.SetTrigger`
- Fix: GroundEnv radius property setting wrong value internally
- Fix: Apply license to unnamed local vite chunk files

## [3.2.9-exp] - 2023-04-23
### Exporter
- Change: ExportInfo big install button should run clean install silently if the project does not exist at all
- Change: Cleanup vite template config

### Engine
- Fix: VideoPlayer not restarting when enable/disable being toggled
- Fix: Builtin serializer for URLs `@serializable(URL)` should ignore empty strings
- Change: set `enabled` to true before calling `onEnable`
- Change: VideoPlayer now deferrs loading of the video until the video should play
- Change: ScreenSharing component now changes cursor pointer on hover to indicate that is can be clicked

## [3.2.8-exp] - 2023-04-23
### Exporter
- Add: DeployToNetlify component
- Change: SceneView now shows server start information
- Change: Improve npm installation logs in Unity and run installations in sequency rather than in parallel
- Change: automatically update workspace title making it easier to work with multiple VSCode editors open
- Change: wait a bit longer before opening browser URL (mainly for safari not refreshing when the vite server takes a bit longer to fully start)
- Change: remove npmdef dependencies in temporary projects (in Library/) when they have not been added explitly in the Unity scene (this is useful when switching many sample projects where one web project is shared for many Unity scenes that might use different local packages - when switching many scenes more and more dependencies would been added to the project altough only few were actually used by the current example scene)
- Fix: font export where font name is "Arial" but font file name is "arial"
- Fix: npmdef dependency path update (remove unnecessary log, only write dependencies if they've actually changed)
- Fix: Catch some timeline export bugs when animation window is open but has no clip

### Engine
- Add: this.context.getPointerUsed(index) and setPointerUsed(index)
- Change: physics now by default receives collisions/triggers between two colliders that are set to trigger

## [3.2.7-exp] - 2023-04-22
### Exporter
- Change: reduce warnings when font style could not be found
- Change: improve switching of scenes in samples repository where packages are added to shared project

### Engine
- Change: ambient light does now look closer to Unity ambient light
- Fix: guard calls to component event methods when the component or object has been destroyed

## [3.2.6-exp] - 2023-04-21
### Exporter
- Fix_ editor sync for enums
- Change: Delete package.lock.json when installing

### Engine
- Add: SceneSwitcher has now option to automatically set scene environment lighting
- Fix: Issue caused by NeedleEngineHTMLElement import from SceneSwitcher
- Change: Allow component to be disabled in awake (e.g. calling `this.enabled = false` in awake callback)
- Change: Export more types e.g. AnimatorStateMachineBehaviour
- Change: VolumeParameter.value should return rawValue (unprocessed)
- Change: rename "rendererData" to "sceneLighting"
- Change: scene lighting must now be enabled explictly when additional scene are being loaded, use `this.context.sceneLighting.enable(...)` with the AssetReference or sourceId of the scene you want to enable

## [3.2.5-exp] - 2023-04-20
### Exporter
- Add: Occluder mode to ShadowCatcher component
- Add: WebXRPlaneTracking

### Engine
- Add: WebXRPlaneTracking
- Add: `<needle-engine loading-style="light">` for a brighter loading screen
- Fix: InputField.onEndEdit should send string
- Change: move webxr into subfolder
- Change: export more types

## [3.2.4-exp] - 2023-04-20
### Exporter
- Add: auto updater for scripts importing types using `engine/src` paths (to skip auto update add `// @noupdate` in the beginning of your file)
- Internal: NpmDef devDependency is now set to current local engine if the current project does use a locally installed engine package

### Engine
- Change: export more types (e.g. `syncField`)
- Fix: PlayerSync
- Fix: Environment lighting
- Fix: license check

## [3.2.3-exp] - 2023-04-20
### Exporter
- Change: bump UnityGLTF dependency to `2.0.0-exp.2`

### Engine
- Fix: VideoPlayer AudioOutput.None should mute video
- Fix: SpriteRenderer applies layer from current object (e.g. for IgnoreRaycast)

## [3.2.2-exp] - 2023-04-19
### Exporter
- Change: Bump engine version

### Engine
- Fix: issue where the environment lighting would be falsely disabled
- Change: minor improvements to initial state of the SceneSwitcher

## [3.2.1-exp] - 2023-04-19
### Exporter
- Remove: New shaders will not be changed anymore
- Change: DriveHelper now runs in background to prevent long stalls on windows call
- Fix: timeline signal asset export

### Engine
- Change: SceneSwitcher clamp option
- Change: timeline signals without bound receiver are now invoked globally on all active SignalReceivers with the specific signal asset
- Change: internal check preventing errors during initialization for projects where the package is falsely added multiple times to the project by importing from internal types directly instead of `from "@needle-tools/engine"`

## [3.2.0-exp] - 2023-04-19
### Exporter
- Add gzip option to DeployToFTP and always enable gzip compression for DeployToGlitch
- Fix minor Unity warnings
- Change: Allow exporting root scene without GltfObject

### Engine
- Add: built-in SceneSwitcher component
- Change: VideoPlayer.playInBackground is set to true by default
- Change: Screensharing should continue playback of receiving video when the sending tab becomes inactive
- Change: log additional information when button events can not be resolved
- Change: AudioSource.playInBackground set to true by default to allow audio playback when tab is not active
- Change: syncField can now take function argument
- Change: Renderer.sharedMaterials can now be iterated using `for(const mat of myRenderer.sharedMaterials)`
- Fix: lightmap not being resolved anymore
- Fix: environment lighting / reflection not switching with scenes
- Fix: progressive texture did not check if the texture was disposed when switching to an unloaded scene resulting in textures being black/missing
- Fix: timeline does enable animator again when pausing/stopping allowing to switch to e.g. idle animations controlled by an AnimatorController
- Fix: changing material on renderer with lightmapping will now re-assign the lightmap to the new material

## [3.1.0-exp.3] - 2023-04-18
### Exporter
- Fix: font export not working when tools helper package was not yet initialized
- Fix: NestedGltf exporting wrong file path

## [3.1.0-exp.2] - 2023-04-18
### Exporter
- Fix: UI font path export

### Engine
- Fix: UI font style resolving

## [3.1.0-exp.1] - 2023-04-18
### Engine
- Fix: RemoteSkybox not being able to load locally reference dimage
- Fix: ParticleSystem sphere scale not being applied anymore
- Fix: WebXRImageTracking url not being resolved

## [3.1.0-exp] - 2023-04-18
### Upgrade Guide
With version 3.x the Needle Engine Unity integration will install Needle Engine from npm instead of installing a separate Unity package and installing it by filepath. This change is an important step to alig the Unity integration with Blender and all future integrations.

After upgrading please make sure to apply the following changes:

- Open your web project package.json and check that the dependency for `three` does not contain an old `file:` path to a previous installation. It may be necessary to change the value from `"file:/path/"` to `""` (empty string) so that the Unity integration can fill in the correct version. You may also remove the explicit dependency to `three` completely if you are not using e.g. react-three-fiber.

- Open the `vite.config.js` and make sure to remove the custom `alias` configuration for `@needle-tools/engine` and `three`

- If you have been using the Unity `ImageReference` class to export images to external files you should change your runtime code to use the new typescript `ImageReference` class as well (using `@serializable(ImageReference)`)

- The `build:dev` script not contain an extra `tsc` compile call

### ⭐ Highlights 
#### **Needle Engine is now installed from NPM**  
Needle Engine in Unity is now also installed from NPM. This is an important step to align Unity, Blender and all future integrations. It will also make it easier to publish projects on platforms like Netlify without having to modify the web project. Please see the changelog for the Upgrade Guide.

#### **WebXR ImageTracking**  
Add the `WebXRImageTracking` component to your scene and assign images to be tracked. Currently requires the `webxr-incubations` chrome flag to be enabled.

#### **Screensharing**
Reliability when making new connections or joining a room with an active screensharing session has been improved.

### Exporter
- Add: automatically update npm dependencies for certain packages (e.g. `@needle-tools/engine`) when a normal Semversion is being used
- Add: initial experimental component import support allowing to import glTF files from Blender into Unity with their components intact (similarly glTF files that have been exported in other Unity projects can now be imported including their components) 
  ↪ **NOTE**: this feature is experimental and not yet production ready. It needs further testing and import does not yet work for all types (known issues where import does not yet work include ParticleSystems and AnimatorControllers where states have missing animation clips)
- Change: bump UnityGLTF dependency to `2.0.0-exp` for import plugin API
- Change: build pipeline tools are now run from an internal package, this removes the need to have a web project setup to export and compress glTF files (e.g. during CI or when using the context menu item on an model or prefab asset)
- Change: remove dependency to extra Unity package and local engine installation.
- Change: paths to external files are not relative to the exported glb (previously they did contain the full path relative to the project root) - this allowed modifications to `needle.config.json` assetsDirectory to work when the folder structure for the deployed version is different to the development structure. NOTE: if you've been exporting external images using `ImageReference` you can now use the new `ImageReference` runtime type to easily resolve them
- Change: Compressed glTF export is now possible without web project
- Change: Full Export now does not restart the server but deletes both Needle Engine as well as vite caches
- Fix: Projects using `needle.config.json` to modify the assets directory are now being built correctly

### Engine
- Add: `ImageReference` type, use to export textures to external files and load them as `img`, `texture` or to get the binary data for e.g. image tracking
- Add: api for `WebXRImageTracking`, this does currently require the ``webxr-incubations`` flag to be enabled
- Add: TiltShift postprocessing effect
- Add: AnimatorController support for negative speed
- Add: `this.context.xrFrame` to get access to the current XRFrame from every lifecycle event method
- Add: `<needle-engine>` loading visuals can now be customized by commercial license holders
- Change: ParticleSystem now has a reset method to allow for clearing state, stop has options for calling stop on Sub-Emitters and to clear already emitted particles
- Change: license check is now baked
- Change: Rename "EngineElement" to NeedleEngineHTMLElement
- Change: disable "Enable Keys" on OrbitControls by default as it conflicts with so many things
- Fix: ParticleSystem circle shape
- Fix: balloon messages are now truncated to 300 characters
- Fix: Screensharing connection setup and start of video playback
- Fix: Screensharing muting now local audio
- Fix: AudioSource does not play again when it did finish and the user switches tabs
- Fix: ParticleSystem prewarm
- Fix: ParticleSystem minMax size, it's currently not supported and should thus not affect rendering
   
## [2.67.16] - 2023-04-13
### Exporter
- Change: Improved handling of error during export if referenced scenes have the same name causing an IOException. Regular export now still continues and an error with some more information is being logged.
- Fix: Exception in attribute drawer
- Fix: Nullreference exception from EditorSync when trying to re-assign a missing script

### Engine
- Change: postprocessing DOF exposes resolution scale and takes device pixel density into account. By default the resolution is slightly lowered on mobile devices

## [2.67.16-pre.1] - 2023-04-12
### Exporter
- Fix: key exception in ExportInfo version check

## [2.67.16-pre] - 2023-04-12
### Exporter
- Change: dont change font name casing

### Engine
- Add: static ``AudioTrackHandler.dispose`` for disposing loaded audio data in timeline
- Fix: issue where only the first audio clip would be played in a timeline with multiple audio clips of the same file
- Change: Text should not change font name casing
- Change: Timeline does now wait for audio and first interaction by default (if any audio track is being used, this can be disabled by setting `waitForAudio` to false on the PlayableDirector component)

## [2.67.15-pre] - 2023-04-12
### Exporter
- Change: Automatically use PBRGraph or UnlitGraph for known shaders when creating a new material
- Change: adding nullchecks to DriveHelper, it seems a drive or drive name can also be null
- Change: show "MODULE NOT FOUND" as error in Unity

### Engine
- Fix: Issue where ControlTrack was not being able to resolve bound timeline
- Fix: issue with font generation where font file name contained a dot

## [2.67.14-pre] - 2023-04-12
### Exporter
- Add: symlink support check (FAT32 and exFAT)

### Engine
- Change: WebXR camera now copies culling mask from main camera
- Fix: WebXRController raycast on all layers
- Fix: WebXR all layers should be visible
- Fix: set pointer position properly on mouse down to prevent jumps in delta
- Fix: respect IgnoreRaycast in physics raycasts
- Fix: issue with CircularBuffer where sometimes the same item was returned twice
- Fix: boxcolliders with scale 0 (such as adding a BoxCollider to a plane) resulted in flipped normals being returned from raycasts
- Fix: parenthesis error in CharacterController
- Fix: issue with mouse vector position being re-used causing delta position being falsely modified

## [2.67.13-pre] - 2023-04-11
### Exporter
- Add: disc formatting check for FAT32

### Engine
- Fix: Animation component settings
- Fix: instanced renderer matrix auto update
- Change: enable shadow casting in instanced rendering when any mesh has castShadow enabled
- Change: export ui pointer events

   
## [2.67.12-pre] - 2023-04-09
### Exporter
- Add: Support exporting immutable scenes (e.g. when referencing a scene in an immutable package)
- Add: SSAO color and luminance influence options
- Fix: handle invalid formatting of vscode workspace json
- Change: Clicking on missing script (uninstalled npmdef / rendered in red font) now pings npmdef (you can double click to still open the script)
- Change: try to find toktx in default install directory on windows and not show warning/error when user has it installed (but not in parth)
- Change: Rename `CustomPostprocessingEffect` to `PostprocessingEffect` to make codegen work (e.g. when creating a custom PostProcessingEffect)

### Engine
- Add: SSAO color and luminance influence options
- Change: postprocessing now exposes effect order

## [2.67.11-pre] - 2023-04-08
### Exporter
- Add: support for exporting skybox and fog settings for referenced scenes
- Fix: ExportInfo getting stuck without install button
- Fix: ProcessHelper remove invalid control characters that might come in from external process breaking logs / showing incomplete information
- Change: Delete broken Unity package install directory (e.g. when hidden Needle Engine directory exists but is empty)
- Change: Show info in "Install Project" Button tooltip what is missing / why it's not installed
- Change: update react template

### Engine
- Add: some checks for WebGPURenderer

## [2.67.10-pre] - 2023-04-06
### Exporter
- Add: tags to samples window
- Change: hold ALT to restart local server (when server is running)
- Change: EditorSync: install ^1.0.0-pre by default

### Engine
- Add vite copy files build plugin
- Fix: PostProcessing not applying effects when enabled for the second time as well as removing earlier hack
- Change: update user-select/touch-action in project templates style.css to prevent accidental iOS selection of canvas
- Change: disable text selection on Needle logo
- Bump three version, see changes below

### Three
- change USDZExporter: pass writer into onAfterHierarchy callback, move onAfterHierarchy callback after scene hierarchy write
- fix USDZExporter: fix exception when trying to process render targets
- fix WebXRManager: Correctly update the user camera when it has a parent with a non-identity transform.

## [2.67.9-pre] - 2023-04-03
### Exporter
- Add: Bug Report upload functionality
- Fix: spritesheet export and display for non-similar sprite shapes
- Change: Improve feedback when nodejs is not installed
- Change: run install in local package.json dependencies
- Change: optimized Spritesheet data export resulting in smaller files

### Engine
- Change: SpriteRenderer material to transparent
- Bump: tools package dependency

## [2.67.8-pre.1] - 2023-03-31
### Exporter
- Bump engine version
- Bump UnityGLTF dependency

### Engine
- Fix: exception when using BoxColliders caused by error in initial activeInHierarchy state assignment

## [2.67.8-pre] - 2023-03-31
### Exporter
- Bump engine version

### Engine
- Fix: vite plugins must have a name
- Fix: activeInHierarchy update when key is undefined (e.g. in r3f context)
- Change: cleanup r3f component

## [2.67.7-pre] - 2023-03-30
### Exporter
- Internal: samples can now have tags

### Engine
- Add: time smoothed delta time
- Add this.context.targetFrameRate
- Fix: enum / type conversion errors
- Fix: CanvasGroup overriding text `raycastTarget` state in event handling causing problems with button events
- Fix: Text z-fighting from invisible ThreeMeshUI frame object
- Change: Canvas `renderOnTop` moved into separate render call to avoid ordering issues and postprocessing affecting overlay UI
- Internal: Move context from engine_setup to engine_context

## [2.67.6-pre] - 2023-03-30
### Exporter
- Fix: reset blurred skybox color

### Engine
- Fix: Postprocessing enforce effect order
- Change: gizmos should not render depth

## [2.67.5-pre] - 2023-03-30
### Exporter
- Fix: issue where context menu export didnt export all known components
- Fix: dont import npmdef types multiple times

### Engine
- Fix: issue where postprocessing did not check composer type (e.g. when using threejs composer instead of pmndrs package)
- Change: Postprocessing now uses stencil layers

## [2.67.4-pre] - 2023-03-28
### Exporter
- Bump engine version

### Engine
- Change: Postprocessing effects value mapping / settings improved (Bloom & ColorAdjustments)

## [2.67.3-pre] - 2023-03-28
### Exporter
- Bump engine version

### Engine
- Fix: issue where progressive textures would not be applied correctly anymore
- Fix: Timeline audio loading on firefox
- Fix: issue where progressive textures with reflection probes wouldn't be applied correctly anymore

## [2.67.2-pre] - 2023-03-28
### Exporter
- Fix: registry component codegen should not generate extra c# types for npmdefs that exist locally
- Change: bump UnityGLTF dependency

### Engine
- Change: calculations for rect transform animation offsets
- Change: Warn if engine element src contains a url without .glb or .gltf

## [2.67.1-pre] - 2023-03-28
### Exporter
- Change: disable soft restart button while installing EditorSync
- Remove: physics debug log when raycasting

### Engine
- Fix: PostProcessing failing to be re-applied after exit XR

## [2.67.0-pre] - 2023-03-27
### ⭐ Highlights 
#### **Editor Live Sync 🔴**  
Immediately see changes made in the Unity Editor in your three.js scene. Add the Needle Editor Sync component to your scene to get started.  

#### **More PostProcessing effects**  
Adding Bloom, Depth of Field, ColorAdjustments, Chromatic Aberration, Screenspace Ambient Occlusion, Vignette, AntiAliasing powered by pmndrs' postprocessing package.

#### **WebAR Camera Background**  
Taking scene screenshots in AR now also includes the camera image. This paves the way for adding custom camera and AR effects in future versions! 

### Exporter
- Add: **EditorSync** component and package to send changes in the editor to the three.js scene. In this first version it can be used to modify material properties, change certain component values at runtime, enable or disable objects as well as to render from the Unity scene camera. To use just add the EditorSync component to scene and click the Install button.
- Add: **More postprocessing effects**: Bloom, ChromaticAberration, ColorAdjustments, DepthOfField, Pixelation, Screenspace Ambient Occlusion, Tonemapping, Vignette. Custom effects can easily be implemented by deriving from the `PostProcessingEffect` base class
- Add: Support to adjust postprocessing effects from the Unity Editor when EditorSync is being used
- Add: **WebARCameraBackground** that can be used to apply effects to the camera image (or capture the image when taking scene screenshots while in AR) using the WebXR Raw Camera access API
- Add: NpmDef `Publish to npm` button
- Add: **Integration of using NpmDefs published to npm**, generating C# components from packages with Needle Engine components installed via npm. This allows to publish packages with Needle Engine typescript components and when installed in a Unity project the corresponding C# components will be generated and discovered on export.
- Change: DeployToFTP should not log error in check if a directory already exists
- Change: Show info in scene view when using SmartExport and scene would not be exported because nothing has changed
- Fix: Remove legacy `camera` field on `SyncCamera` component
- Fix: Typo in allowed camera fields causing `ARBackgroundAlpha` to not be exported
- Fix: wrong warning for NpmDef being missing when name was containing a `.` 
- Fix: Issue in welcome window where certain URLs were opened twice
- Fix: Issue where assembly reload lock would not be unlocked again 
- Other: Internal cleanup and deletion of a lot of legacy code that was originally used to build three.js scenes from javascript codegen
- Other: Improved menu items order and wording

### Engine
- Add: `this.gameObject.destroy` as a shorthand for `GameObject.destroy(this.gameObject)`
- Add: support for camera `targetTexture` to render into a RenderTexture (when assigned to e.g. the main camera in Unity)
- Add: utility methods to toplevel engine export (for example `getParam`, `delay`, `isMobileDevice`, `isQuest`)
- Add: first version of usage tracking of materials, textures and meshes. This is off by default and can be enabled using the `?trackusage` url parameter. When enabled `findUsers` can be used to find users of an object.
- Add: pmnders postprocessing package
- Change: improved PlayerState component and added event for PlayerState events for owner change (to properly listen to first assignment)
- Change: `AssetReference.unload` now calls dispose to free all resources
- Change: `WebXR` component now has static field for modifying optionalFeatures 
- Change: `Physics.RaycastOptions` now have a `setLayer` method to simplify setting the correct layer mask (similar to Unity's layers to enable e.g. just layer 4 for raycasting)
- Change: `RemoteSkybox` now requests to re-apply clear flags on main camera when disabling to restore previous state.
- Fix: issue where component instances were created using wrong method causing arrow functions to be bound to the wrong instance
- Fix: `@syncField` now properly handles destroy of an component
- Fix: react-three-fiber template
- Fix: ParticleSystem prewarm safeguard and additional checks where emission is set to 0
- Fix: Timeline only playing first audio clip
- Fix: Issue where scene with multiple root glbs cross-referencing each other were not being resolved correctly in all cases
- Fix: Progressive textures not being loaded when using tiling
- Fix: Text UI anchoring

## [2.66.1-pre] - 2023-03-16
### Exporter
- Add: additional option to Lightmap format dialogue to set the right format when exporting using unsupported lightmapping quality setting
- Fix: AutoInstall nullref if installing a package in a scene that has no Needle Engine project
- Fix: BRP `shadowBias` conversion
- Fix: unassigned methods in Unity Events ("No Function") were throwing an exception on export
- Fix: change detection (Smart Export) for referenced assets was not checking for changes in some cases
- Change: component header link now has file extension
- Change: Bump UnityGLTF dependency
- Change: allow specifying double jump force separately in CharacterController

### Exporter
- Add: `sphereOverlapPhysics` function for more accurate sphere overlap detection using rapier
- Fix: Gizmos should be excluded from being hit by raycasts
- Fix: Gizmo sphere radius was twice the desired size
- Fix: Physics now prevent negative collider scale
- Fix: renderer instancing auto update when using multi material objects
- Change: Show warning that stencil and instancing doesnt work together at the moment

## [2.66.0-pre.1] - 2023-03-14
### Exporter
- Fix: issue where scene assets wouldnt be exported properly anymore
- Fix: issue with very first web project installation in a new unity project

## [2.66.0-pre] - 2023-03-14
### Exporter
- Fix: AssetReferenceResolver should not export references on a asset root as separate glbs
- Fix: skip serialization for `transform` property on components
- Fix: Updater should trim leading whitespace when testing if line is import and needs to be updated
- Fix: npmdef button "Add to project" / "Remove from project" not updating workspace
- Fix: deprecated import in basic_component template
- Fix: NestedGltf should not dont traverse into EditorOnly objects
- Change: Export cache: check if the parent context is null (in case of e.g. export from prefab)
- Change: Prevent exporter from re-exporting glb with the same name multiple times per run. Instead print a warning. This can be caused by e.g. using nested gltfs or GltfObject components where the GameObject has the same name as another previously exported GltfObject
- Change: Full Export does now also kill local server, add "force" to search for node processes running current web project
- Change: Updater can now run for immutable package when in hidden directory
- Remove legacy transform.guid code that slowed down export in scenes with many objects

### Engine
- Add: particle system prewarm support
- Add: `poseMatrix` argument in WebARSessionRoot.placedSession event
- Add: MeshCollider minimal support for submeshes (they can be added but currently not removed from the physics world)
- Fix: debug_overlay error when rejection reason is null
- Fix typo beginListenBinrary → beginListenBinary
- Fix: particle system staying visible after disabling gameObject
- Fix particle system not finding subemitter in certain cases
- Fix: particles with subemitters and trails and disabled "die with particle" should emit subemitter when the particle dies (and not when the trail dies)
- Fix: Loading overlay now hides when loading is done and the first frame has finished rendering
- Change: rename networking `stopListening` to `stopListen` for consistency
- Change: addressables allow up to 10k instantiations per frame before aborting
- Change: set material shadowSide to match side
- Change: generate poster image with 1080x1080 px and add `og:image:width` and `og:image:height` meta tags

## [2.65.2-pre] - 2023-03-11
### Exporter
- Fix: custom shader export of uv2 etc
- Fix additional data button should now show up on model importer
- Change: dont run Updater automatically if `allowProjectFixes` is disabled (in Needle settings)
- Change: dont run Updater for script files in PackageCache
- Change: custom shader export now caches previously exported shaders (per export) when having a scene with multiple materials using the same custom shader
- Change: log warning when exporting with gzip disabled (it is disabled by default, please check your Build Settings)

### Engine
- Change: custom shaders should not log warning for unsupported ``OrthoParams`` shader property 
- Change: Animator methods starting with uppercase are marked as deprecated because UnityEvent methods are now exported starting with lowercase letter, added lowercase methods

## [2.65.1-pre] - 2023-03-10
### Exporter
- Change: Updater now locks assemblies during auto-update to avoid components from recompiling and aborting export or server start

### Engine
- Fix: ParticleSystem using `min/max` size in the renderer module is now minimally handled
- Fix: ParticleSystem emission when using local space with scaled parents
- Fix: ParticleSystem not finding SubEmitter systems
- Fix: ParticleSystem simulation speed not being applied to gravity and initial speed
- Fix: RemoteSkybox not resolving relative url correctly (when assigning a cubemap in the editor)

## [2.65.0-pre.1] - 2023-03-10
### Exporter
- Fix: issue where `<needle-engine>` element was not yet in the DOM when queried by exporter codegen which caused the paths to not be assigned and the engine to not load

### Engine
- Fix: issue where `<needle-engine>` element was not yet in the DOM when queried by exporter codegen which caused the paths to not be assigned and the engine to not load

## [2.65.0-pre] - 2023-03-09
### Exporter
- Add: Updater to fix wrong import paths due to change in engine package structure (`@needle-tools/engine/src/...`)
- Change: codegen for loading the exported glbs in main scene is now simplified, removing previous and legacy code completely. It now just collects all exported files in an array and sets that as the `src` attribute on the `<needle-engine>` web component. 
- Change: project settings should not show warning for Engine package not being installed when the current scene is not a web project
- Change: engine package should only be automatically installed on update when the current scene is a web project
- Change: type imports now generate without an extension to fix distributed `lib` imports
- Fix: error where unresolved package.json variable did cause IllegalChar exception
- Fix: improved npmdef error handling caused by the hidden package being missing which might happen if a user copies the npmdef to another Unity project but does not copy the hidden package folder (ending with ~). Such cases are now also properly displayed in the ExportInfo component
- Fix: BugReporter should not print error message about toktx not being installed when collecting project information

### Engine
- Add: runtime checks for recursive loading to prevent it from breaking
- Change: internal duplicate active state on `Object3D` has been removed, instead `visible` is used. This was previously a workaround for the `Renderer` setting the visible state when being enabled or disabled but this has been changed in a previous version and it now only sets a flag in the object's Layers instead (which allows for a single object in the hierarchy to not being rendered by setting `Renderer.enabled = false` while objects in the child hierarchy stay visible) 
- Change: `<needle-engine>` src attribute can now also take an array of glbs to load. This simplifies codegen done by exporters and also prevents errors due to bundler optimizations as well as being easier to understand. Runtime changes of the `src` attribute (especially when using arrays of files) have not been tested for this release. Networking for `src` changes has been removed in this release. 
- Change: move engine into src subfolder. All paths to explicit script files are now `@needle-tools/engine/src/...`
- Change: poster screenshot will be taken after 200ms now
- Change: canvas default set to false for castShadow and receiveShadow
- Change: Remote skybox should not set `scene.background` when in XR with pass through device (e.g. when using Quest Pro AR or AR on mobile)
- Fix: issue where ColorAdjustments Volume effect was applied with `active` set to false
- Fix: `Light` not being enabled again after disabling the component once

## [2.64.0-pre] - 2023-03-07
### Exporter
- Add: dependency to `csharp to typescript` package. 
  > This allows to quickly create typescript skeleton or stub components from existing csharp code
  > Created components try to import used types if known, create fields with `@serializable` decorations and methods (the method body needs to be implemented manually)
  > Use the context menu item on csharp script assets or on components in the scene
- Add: When your package.json contains a script named `install` the exporter will invoke this script instead of directly running `npm install`. This allows running projects that e.g. require `yarn install`
- Add: Export audio volume for timeline clip (Timeline audio track settings are not yet supported)
- Fix: Clean install when using project paths starting with `Packages/` or `Assets/`
- Fix: Issue where the `enabled` property of some component types was not exported anymore (e.g. colliders)
- Fix: Issue with license import when using vite 2 caused by BOM

### Engine
- Add: `PlayableDirector` now correctly applies timescale
- Add: `PlayableDirector.speed` property allowing to play the timeline at different speeds or even reverse (reversed audio playback is not supported at the moment)
- Add: `Physics.enabled` property for disabling physics updates. This also prevents any collider shapes to be created
- Add: `this.gameObject.transform` property to ease getting started for devs coming from Unity. This property is merely a forward to `this.gameObject` and shouldnt really be used. The property description contains information and a [link to the docs](https://fwd.needle.tools/needle-engine/docs/transform) with further information.
- Fix: instanced materials using progressive loading are now correctly updated
- Fix: Timeline animation tracks now disable the `Animator`. This fixes cases where two animations were played at the same time. When the PlayableDirector is paused or stopped the animator state is reset
- Fix: License styles leaking into website
- Fix: Timeline audio not stopping correctly at end of timeline when set to hold
- Change: improve instancing updates, instanced objects now auto update detect matrix changes. This includes improvements of instancing when used with `Animation` components
- Change: set particle system layers to `IgnoreRaycast` to not receive wrong click events on batched particle meshes
- Change: Timeline audio is now loaded on evaluation. Only clips in a range of 1 second to the current time are loading. To manually trigger preload of audio you can invoke `loadAudio` with a time range on audio tracks of a timeline

## [2.63.3-pre] - 2023-03-03
### Exporter
- Change: ExportInfo should install should not show too much information when ALT is pressed
- Change: project generator should not replace version for needle engine in package.json paths
- Fix: project generator should insert package path if the dependency is an empty string

### Engine
- Fix: engine published to npm was missing vite plugins

## [2.63.2-pre] - 2023-03-03
### Exporter
- Add: ExportInfo context menu `Internal/Move Project` to move a web project
- Change: allow web projects in Assets/ and Packages/ directories (when they're in a hidden folder like `Assets/MyProject~` or `Packages/com.my.package/MyProject~`)
- Change: ignore build.log
- Change: bump UnityGLTF dependency
- Fix npmdef open button, rename "path" to "name" because that's what it is
- Fix: deploy to ftp when server name starts with ftp.

### Engine
- Fix: license styling in some cases
- Fix: duplicatable + draggable issue causing drag to not release the object (due to wrong event handling)
- Fix duplicatable + deletion not working properly
- Fix: timeline breaking when time is set to NaN

## [2.63.1-pre] - 2023-03-02
### Engine
- Add: components now have `up` and `right` vectors (access via ``this.up`` from a component)
- Fix: import of license and logo for npm package

## [2.63.0-pre] - 2023-03-01
### Exporter
- Add: licensing information

### Engine
- Add: licensing information
- Add: logo to loading display
- Change: VideoPlayer now exposes VideoMaterial
- Change: Screencapture now only accepts clicks with pointerId 0 (left mouse button, first touch) to toggle screen capture
- Change: expose physics gravity setting `this.context.physics.gravity`

## [2.62.2-pre] - 2023-02-27
### Engine
- Add: support for `camera-controls` attribute on `<needle-engine>` element. When added it will automatically add an `OrbitControls` component on start if the active main camera has no controller (you can assign custom controllers by calling `setCameraController` with the camera that is being controlled)
- Fix: rare error in extension_util
- Fix: timeline preExtrapolation setting
- Fix: disabling Light component should turn off light
- Fix: animating camera fov, near or far plane
- Fix: threejs layer patch for Renderer visibility is now always applied
- Fix: UI runtime instantiate of canvas from templates in scene
- Fix: UI text did not update shadow-component-owner on font loading
- Fix: UI EventSystem raising click event multiple times in some cases
- Fix: UI Text raycast now respects object layer (NoRaycast)
- Fix: UI duplicate pointerUp event
- Fix: UI highlighting getting stuck in wrong state sometimes

## [2.62.1-pre] - 2023-02-23
### Exporter
- Add: FontAdditionalCharacters component to allow to specifiy additional fonts to be included in a font atlas
- Fix: missing animationclip in director caused export exception
- Fix: ComponentGenerator not watching subdirectories in `src/scripts`
- Fix vite plugin not using config codegen directory
- Fix vite plugin assuming <needle-engine> web component in index.html and producing error if not found
- Change: bump UnityGLTF to 1.22.4-pre

### Engine
- Fix: pause wasn't evaluating and thus not pausing audio tracks
- Fix: debug overlay styles were not properly scoped and affected objects inside needle-engine tag
- Fix: Addressables wrong recursive instantiation error
- Fix: UI not showing fully when setting active at runtime
- Change: timeline tracks are now created immediately but their audio clips are deferred until audio is allowed

## [2.62.0-pre] - 2023-02-13
### Exporter
- Add: Meshopt decoder support for engine-loaded glTF files
- Add: better logs when running in headless mode and operations fail due to non-installed packages
- Fix: nullrefs when saving in scenes that don't have ExportInfo components
- Fix: explicit texture compression "None" resulted in wrong compression applied in some cases
- Change: Update UnityGLTF dependency including fixes for specular extension roundtrips, importer improvements

## [2.61.0-pre] - 2023-01-30
### Exporter
- Add: batch export now allows `-scene` arg to point to a prefab or asset and adds `-outputPath` argument to define the path and name of the exported glb(s)
- Fix: rare vite plugin poster error when include directory does not exist
- Fix poster incorrectly being generated when building
- Fix: Dialog that shows up when lightmap encoding settings are wrong now shows up less often
- Fix: serialized npmdefs with wrong paths are not automatically repaired or cleaned up from serialized data
- Change: Bug reporter now assumes .bin next to .gltf is a dependency until .bin is properly registered as a dependency in Unity
- Change: bump gltf-pipeline package fixing a rare bug where toktx could not be found

### Engine
- Add: canvas applyRenderSettings
- Add: progressive support for particle system textures

## [2.60.4-pre] - 2023-01-27
### Exporter
- Change: dont reload page while build preview is in progress (when running ExportInfo/Compress/PreviewBuild)
- Fix: bump build pipeline package to fix issue where texture compression settings were taken from wrong texture
- Fix: vite reload plugin sometimes preventing reload

### Engine
- Fix UI prefab instantiate throwing error at runtime
- Change: show warning when unsupported canvas type is selected
- Change: show warning when trying to use webxr features in non-secure context

## [2.60.3-pre] - 2023-01-26
### Exporter
- Fix register type error when component class with the same name exists multiple times in the same web-project in different files [issue 49](https://github.com/needle-tools/needle-engine-support/issues/49)
- Fix: NeedleAssetSettingsProvider and simplify setting texture settings on import like so:
   ```csharp
   if (NeedleAssetSettingsProvider.TryGetTextureSettings(assetPath, out var settings))
   {
      settings.Override = true;
      settings.CompressionMode = TextureCompressionMode.UASTC;
      NeedleAssetSettingsProvider.TrySetTextureSettings(assetPath, settings);
   }
   ```

### Engine
- Fix: camera fov for blender export allowing fieldOfView property to be undefined, where the fov should be handled by the blender exporter completely.

## [2.60.2-pre.1] - 2023-01-26
### Exporter
- Fix: remove accidental codice namespace using in 2022

## [2.60.2-pre] - 2023-01-26
### Exporter
- Add: Api to access texture compression settings (use `NeedleAssetSettingsProvider`)
- Add pre-build script to run tsc
- Fix: cubemap export fallbacks to LDR format if trying to export cubemap on unsupported build target (e.g. Android)
- Fix: project paths replacement when path has spaces
- Fix: remove global tsc call before building

### Engine
- Fix: particle textures being flipped horizontally

## [2.60.1-pre.1] - 2023-01-25
### Exporter
- Fix: Smart export file size check if file doesnt exist

## [2.60.1-pre] - 2023-01-25
### Exporter
- Change: Make cubemaps use correct convolution mode, downgrade error to warning
- Change: Cubemap warning should not show for skybox
- Change: Smart export check if file exported was < 1 kb in which case we always want to re-export
- Change: vite server plugin now communicates scheduled page reload to client
- Change: bump gltf extensions package dependency
- Fix: vite reload on changed codegen files (it should not reload there)

### Engine
- Change: export Mathf in `@needle.tools/engine`

## [2.60.0-pre] - 2023-01-25
### Exporter
- Add: check allowed cubemap convolution types and log error if that doesn't match
- Change: Remove `backgroundBlurriness` setting on RemoteSkybox (should be controlled on the camera)
- Fix: ExportInfo now doesnt display packages as `local` on OSX anymore
- Fix: GltfReference nullref when exporting via context menu, always ignore smart export for context menu exports
- Fix NEEDLE_gltf_dependencies extension causing gltfs to be invalid
- Fix: Platform compiler errors

### Engine
- Add: Particles support for horizontal and vertical billboards
- Add: Timeline now supports reversed clip (for blender timeline)
- Change: bump gltf pipeline package dependency adding support for global `NEEDLE_TOKTX` environment variable
- Change: timeline clip pos and rot are now optional (for blender timeline)
- Fix: when first loading a gltf pass guidsmap to components (for blender timeline)
- Fix: scrubbing TimelineTrack scrubs audio sources as well now
- Fix: stencils for multimaterial objects

## [2.59.3-pre] - 2023-01-21
### Exporter
- Add: particles basic support for on birth and ondeath subemitter behaviour
- Change: run typescript check before building for distribution
- Fix: saving referenced prefab with auto-export causing export to happen recursively
- Fix: improve vite reloading, generate needle.lock when exporting from referenced scene or prefab to prevent reloading while still exporting
- Fix: vite reloading scripts for usage with vuejs

### Engine
- Add: particles basic support for on birth and ondeath subemitter behaviour

## [2.59.2-pre.1] - 2023-01-20
### Engine
- Fix: issue where click on overlay html element did also trigger events in the underlying engine scene  

## [2.59.2-pre] - 2023-01-20
### Exporter
- Add: save in Prefab Mode does now attempt to re-export currently viewed prefab (similarly to how referenced scenes will re-export if they are referenced in a currently running web project)
- Change: EXR textures are now exported zipped (UnityGLTF)
- Change: OrbitControls now use damping (threejs)
- Change: default mipmap bias is now -0.5 (threejs)
- Change: DeployToFTP inspector now shows info if password is missing in server config asset 
- Change: Bump dependencies 
- Fix: export of human animation without transform when discovered from animatorcontroller should not cause errors
- Fix: handle cubemap export error on creating texture when Unity is on unsupported platform
- Fix: context click export for nested element in hierarchy
- Fix: bump gltf-transform-extensions package fixing a failure when using previously cached texture data but not setting the texture mime-type which caused errors at runtime and the texture to not load
- Fix: timeline now skips exporting clips with missing audio assets in AudioTrack
- Fix: subasset importer throwing nullref when selecting subassets from multiple assets and modifying their import settings
- Fix: Unity build being blocked by BuildPlayerHandler
- Fix: Unity build errors

### Engine
- Add: SpectatorCam.useKeys to allow users to disable spectator cam keyboard input usage (`f` to send follow request to connected users and `esc` to stop following)
- Change: expose SyncedRoom.RoomPrefix

## [2.59.1-pre] - 2023-01-18
### Exporter
- Fix: export error where object was being exported twice in the same event as transform and as gameObject due to self-referencing

## [2.59.0-pre] - 2023-01-18
### Exporter
- Add: Smart Export option, which will not re-export referenced prefabs or scenes if they didnt change since the last export (enable via ProjectSettings/Needle) improving export speeds significantly in certain cases. This option is off by default
- Add: lock file to prevent vite from reloading while export is still in process
- Add: warning when older nodejs version fails because of unknown ``--no-experimental-fetch`` argument
- Change: Some methods of DeployToFTP can now be overriden to customize uploading
- Change: TextureCompressionSettings can now be overriden to customize compression settings
- Change: Minor optimization of exported json, removing some unused data to reduce output size slightly for large or deeply nested projects 
- Fix: Issue where vite reload plugin did sometimes not trigger a reload after files have changed
- Fix: Issue where prefab containing GltfObject did not create a nested gltf to be lazily loaded
- Fix: Issue where nested gltf would cause IOException when it had the same name as an glb in the parent hierarchy
- Fix: TextureSizeHandler not being used when not added to GltfObject. It can now be added to any object in the scene to globally clamp the size of exported textures.
- Fix: Export of default font in 2022 (LegacyRuntime)
- Fix: AnimatorOverrideController is now properly ignored (currently not supported) instead of being serialized in a wrong/unexpected format which did cause errors at runtime
- Fix: Issue where DeployOnly did cause already compressed assets in output directory being replaced by uncompressed assets
- Fix: Texture compression set to ``Auto`` did not be properly export
- Fix: issue where default compression wasnt applied anymore when no specific compression settings where selected / setup anywhere
- Fix: Context menu export with compression from Project window now runs full compression pipeline (applying progressive transformation as well as compression) 
- Remove: Experimental SmartExport option on GltfObject

### Engine
- Add: AssetReference.unload does now dispose materials and mesh geometry
- Add: ``setParamWithoutReload`` now accepts null as paramValue which will remove the query parameter from the url
- Change: timeline does now skip export for muted tracks
- Change: OrbitControls can now use a custom html target when added via script and before enable/awake is being called (e.g. ``const orbit = GameObject.addNewComponent(this.gameObject, OrbitControls, false); orbit.targetElement = myElement``)
- Change: Input start events are now being ignored if a html element is ontop of the canvas
- Fix: use custom element registry to avoid error with `needle-engine element has already been defined`
- Fix: timeline not stopping audio on stop
- Fix: input click event being invoked twice in certain cases
- Fix: ParticleSystem start color from gradient
- Fix: ParticleSystem not properly cleaning up / removing particles in the scene in onDestroy
- Fix: ParticleSystem velocity now respects scale (when mode is set to worldscale)

## [2.58.4-pre] - 2023-01-14
### Exporter
- Update template vite config to improve reloading (you can update the vite config in existing projects via ExportInfo Context Menu > Update vite config) 

### Engine
- Update gltf-extensions package dependency

## [2.58.3-pre] - 2023-01-13
### Exporter
- Change: Update UnityGLTF dependency including fixes for gltf texture imports 
- Fix: run install on referenced npmdefs for distribution builds when packages have changed
- Fix: catch WebRequest invalid operation exception

## [2.58.2-pre.1] - 2023-01-13
### Exporter
- Fix: compiler error on osx and linux

## [2.58.2-pre] - 2023-01-12
### Exporter
- Add: start support for targeting existing web projects
- Add: support for animating color tracks when only alpha channel is exported
- Change: use vite for internal compiling of distributable npm package of needle-engine 
- Change: remove scene asset context menu override
- Change: bump UnityGLTF dependency
- Change: run compression commands when building web project from Unity
- Fix: OSX component compiler commands not being executed when containing spaces
- Fix: Linux using sh for terminal commands instead of zsh 
- Fix: Blendshape normals export
- Fix: error in vite plugin generating poster image
- Fix: Embedded assets for 2022 could not select Needle Engine compression settings
- Fix: Texture MaxSize setting not being passed to UnityGLTF
- Fix: Occasional error when exporting fog caused by component not being in runtime assembly
- Fix: Component compiler should update watcher when project directory changes
- Fix: Export of color alpha animation
- Fix: Light shadow bias settings export for URP when light didnt have UniversalAdditionalLightData component

### Engine
- Change: use draco and ktx loader from gstatic server by default
- Change: reduce circular dependencies
- Fix: Reflectionprobe selecting wrong probe when multiple probes had the exact same position

## [2.58.1-pre] - 2023-01-09
### Exporter
- Fix: light default shadow bias values
- Fix: template vite config
- Fix: timeline exported from prefab was sometimes not exported correctly (due to Playable graphs) - this is now fixed by rebuilding the graph once before export

### Engine
- Add: Prewarm rendering of newly loaded objects to remove lag/jitter when they become visible for the first time
- Change: renderer now warns when sharedMaterials have missing entries. It then tries to remap those values when accessing them by index (e.g. when another component has a material index serialized and relies on that index to be pointing to the correct object)

## [2.58.0-pre] - 2023-01-09
### Exporter
- Add hot reload setting (requires vite.config to be updated which can be done from ExportInfo context menu)
- Add fog export

### Engine
- Add: EventSystem input events (e.g. IPointerClick) are now invoked for all buttons (e.g. right click)
- Add: Hot reload for components

## [2.57.0-pre] - 2023-01-07
### Exporter
- Add: meta info export for vite template
- Add: HtmlMeta component to allow modification of html title and meta title/description from Unity
- Add: Support for poster image generation
- Change: Use custom vite plugin for gzip setting

### Engine
- Remove: Meshline dependency
- Fix: Testrunner Rigidbody import error

## [2.56.2-pre] - 2023-01-06
### Exporter
- Fix: BuildPlatform option for Unity 2021 and newer
- Fix: npm install command for npm 9
- Fix: Light shadowBias settings for Builtin RP
- Change: Include npm logs and version info in bug report logs

### Engine
- Change: Component.addEventListener argument can now derive from Event

## [2.56.1-pre] - 2023-01-05
### Exporter
- Add: initial batch mode / headless export support, can be invoked using `path/to/Unity.exe -batchmode -projectPath "path/to/project" -executeMethod Needle.Engine.ActionsBatch.Execute -buildProduction -scene "Assets/path/to/scene.unity"`, use `-debug` to show Unity console window during process
- Fix: sample window now locks assembly reload while downloading until after installation has finished, show progress report for user feedback 
- Fix: sample window not respecting user cancel

### Engine
- Fix: UI setting Image.sprite property did apply vertical flip every time the sprite was set

## [2.56.0-pre] - 2023-01-04
### Exporter
- Add: mesh compression support
- Add: compression settings for textures and meshes in embedded assets (e.g. an imported fbx or glb now has options to setup compression for production builds)
- Change: Bump UnityGLTF dependency adding caching of exported image data to speed up exports for texture heavy scenes

### Engine
- Add: file-dropped event to DropListener
- Add: UI image and raw image components now support updating texture/sprite at runtime
- Change: Bump needle gltf-transform extensions package adding mesh compression and caching for texture compression leading to significant speedups for subsequent production builds (only changed textures are re-processed)
- Fix: light normal bias defaults

## [2.55.2-pre] - 2023-01-02
### Exporter
- Change: log warning if node is not installed or can not be found before trying to invoke component compiler
- Fix: handle `node` commands similarly to how `npm` commands work

### Engine
- Add: Rigidbody.gravityScale property
- Add: Gizmos.DrawArrow method
- Add: Rigidbody.getAngularVelocity method
- Fix: Mesh collider center of mass

## [2.55.1-pre] - 2022-12-30
### Exporter
- Add: Command Tester window
- Fix: error on OSX when nvm directory does not exist

### Engine
- Add: Warning when serialized component field name is starting with uppercase letter
- Change: bump component compiler dependency
- Fix: Particle rotation over lifetime
- Fix: Particles should not emit when emission module is disabled
- Fix: LODGroup breaking rendering when used with multi-material objects or added on mesh to be culled directly

## [2.55.0-pre] - 2022-12-21
### Exporter
- Add: PhysicsMaterial support
- Fix Spline export
- Fix: Renderer not exporting enabled bool
- Fix: Dev <> Production build flip in DeployToGlitch component

### Engine
- Add: PhysicsMaterial support
- Add: ``Time.timesScale`` factor
- Change: VideoPlayer exposes underlying HTML video element
- Change: EffectComposer check if ``setPixelRatio`` method exists before calling
- Change: WebARSessionRoot and Rig rotation
- Fix: WebXRController raycast line not being visible in Quest AR
- Fix: Renderer that is disabled initially now hides object
- Fix: Some ParticleSystem worldspace settings when calling emit directly

## [2.54.3-pre] - 2022-12-19
### Exporter
- Change: OSX now automatically trys to detect npm install directory when installed using nvm

## [2.54.2-pre] - 2022-12-19
### Exporter
- Change: Improve SamplesWindow adding search field and better styling
- Change: Rename ``UseProgressiveTextures`` to ``ProgressiveTextureSettings``
- Change: Progressive texture loading can now be disabled completely using ProgressiveTextureSettings component
- Change: Only generate progressive loading textures when building for distribution / making a build for deployment
- Change: Remove internal ``ObjectNames.NicifyVariableNames`` which caused unexpected output for variable names starting with `_`
- Change: Remove unused NavMesh components
- Fix: Help menu item order
- Fix: Sample window styling for single column
- Fix: Initial project generation does now run installation once before replacing template variables which previously caused errors because the paths did not yet exist.

### Engine
- Change: debug parameter can now take ``=0`` for disabling them (e.g. ``freecam=0``)
- Fix: InputField opens keyboard on iOS

## [2.54.1-pre] - 2022-12-15
### Engine
- Fix: issue with progressive loading, loading files multiple times if a texture was used in multiple materials/material slots. This was causing problems and sometimes crashes on mobile devices 
- Fix: balloon messages using cached containers didnt update the message sometimes and displaying an old message instead

## [2.54.0-pre.1] - 2022-12-14
### Engine
- Fix: bump gltf extensions package fixing issue with progressive texture loading when multiple textures had the same name 

## [2.54.0-pre] - 2022-12-14
### Exporter
- Add: custom texture compression and progressive loading settings for Needle Engine platform to texture importer
- Add: support for webp texture compression
- Add: tsc menu item to manually compile typescript from Unity 
- Add: support for spritesheet animationclips
- Add: menu item to open bug reports location
- Change: sort component exports by name
- Change: update UnityGLTF version
- Fix: issue with wrong threejs path being written to package.json causing button "Run Needle Project Setup" to appear on ExportInfo

### Engine
- Add: start and end events for progressive loading
- Add: USDZExporter events for button creation and after export
- Change: apply WebARSessionRoot scale to exported object, e.g. if scene is scaled down on Android it should receive the same scale when exporting for Quicklook
- Fix: process reflection probe update in update event to avoid visible flickr after component enabled state has changed

## [2.53.3-pre.1] - 2022-12-12
### Engine
- Fix: implement ButtonColors

## [2.53.3-pre] - 2022-12-12
### Exporter: 
- Fix: InvalidCastException when trying to export AnimatorOverrideController

### Engine
- Add: GroundProjection appyOnAwake to make it possible to just use it when the environment changes via remote skybox and not apply it to the default skybox
- Change: more strict tsconfig
- Change: allow overriding loading element
- Fix: apply shape module rotation to direction
- Fix: ParticleSystem world position not being set when shape module was disabled

## [2.53.2-pre] - 2022-12-09
### Exporter
- Change: order generated types alphabetically
- Fix: engine export codegen should only run in local dev environment

## [2.53.1-pre] - 2022-12-08
### Exporter
- Fix OSX bugs regarding nvm and additional search paths not being used correctly

## [2.53.0-pre] - 2022-12-08
### Exporter
- Add: progressive build step is now separated from Unity Exporter and runs in the background to transform exported gltfs to be progressively loaded. That requires a ``UseProgressiveTextures`` component in the scene. Textures can be excluded from being processed by adding a ``noprogressive`` AssetLabel
- Add: USDZExpoter component which will display ``Open in Quicklook`` option when running on iOS Safari instead of WebXR not supported message.
- Add: Automatically update @types/three in referenced project dependencies to match types declared in core engine
- Change: Only open dist directory after building when not deploying to either FTP or Glitch
- Change: Display toktx message about non-power-of-two textures as warning in Unity
- Change: DeployToFTP inspector now behaves just like DeployToGlitch (using ALT to toggle build type)

### Engine
- Add: InstantiateIdProvider constructor can now take string too for initializing seed
- Add: USDZExpoter component enabling ``Open in Quicklook`` option by default when running on iOS Safari
- Fix: Light intensity
- Fix: Add workaround texture image encoding issue: https://github.com/needle-tools/needle-engine-support/issues/109
- Fix: OrbitControls.enableKeys
- Fix: Remove warning message about missing ``serializable`` when the reference is really missing
- Fix: ``context.domX`` and ``domY`` using wrong values when in AR mode

## [2.52.0-pre] - 2022-12-05
### Exporter
- Add initial support for Spritesheet export (spritesheet animationclip export will be added in one of the next releases)
- Add: RemoteSkybox environmentBlurriness setting
- Add: environmentBlurriness and -Intensity setting to CameraAdditionalData component
- Update templates tsConfig adding skipLibCheck to avoid errors when types/three have errors
- Change: Dont open dist folder when deploying to a server like FTP or Glitch
- Change: Start server now checks vite.config for configured port
- Change: adjust materials to UnityGltf/PBRGraph for better cross-pipeline compatibility

### Engine
- Add iOS platform util methods
- Add ``?debugrig`` to render XRRig gizmo
- Add support for Spritesheet Animation
- Add: EventTrigger implementations for onPointerClick, onPointerEnter, onPointerExit, onPointerDown, onPointerUp
- Add: RemoteSkybox environmentBlurriness setting
- Fix: Renderer reflection probe event order issue not applying reflection probes when enabling/disabling object because reflection probes have not been enabled
- Fix: remove log in ParticleSystemModules

## [2.51.0-pre] - 2022-11-30
### Exporter
- Add: basic texture compression control using ``ETC1S`` and ``UASTC`` Asset Labels, they can be added to either textures or exported Asset (for example gltf asset) to enforce chosen method in toktx (production builds) 
- Change: Improve BugReporter
- Fix: DefaultAvatar XRFlags
- Fix: Progressive texture export (high-res glb) not using selected texture compression method

### Engine
- Change: remove nebula, dat.gui and symlink package dependencies
- Change: Light does not change renderer shadowtype anymore
- Change: update threejs to 146
- Change: update threejs types
- Change: Screencapture should not start on click when not connected to networked room
- Change: WebXR returns ar supported when using Mozilla WebXR
- Fix DragControls drag interaction not disabling OrbitControls at right time
- Fix physics collider position in certain cases
- Fix Rigidbody not syncing physics position when parent transform changes
- Fix Timeline awake / active and enable
- Fix: OrbitControls calulcating target position with middle mouse click in worldspace instead of localspace causing wrong movement when parent is transformed
- Fix: Raycast in Mozilla WebXR / using window sizes instead of dom element sizes
- Fix input with scrolled window
- Fix: destroy local avatar on end of webxr session (https://github.com/needle-tools/needle-engine-support/issues/117)
- Fix: WebXRAvatar setting correct XRFlags

## [2.50.0-pre] - 2022-11-28
### Exporter
- Add: Skybox export checks to ensure texture is power of two and not bigger than 4k when exported using hdr
- Add: RemoteSkybox component to allow referencing local image texture
- Add: Set UASTC compression to sprite textures to improve production build quality for UI graphics

### Engine
- Add warning to Light when soft shadows change renderer shadow type
- Add: RemoteSkybox can now load jpg and png textures as skybox
- Change: Instantiate does now copy Vector, Quaternion and Euler objects to ensure multiple components dont share the same objects
- Fix: AnimatorController causes threejs error when creating empty animationclip (Blender) 
- Fix: AnimatorController error when transition has no conditions array (Blender)

## [2.49.1-pre] - 2022-11-25
### Engine
- Add circular instantiation check to AssetReference
- Allow filtering ``context.input.foreachPointerId()`` by pointer types (e.g. mouse or touch)
- Fix typescript error in particle system module function (happened only when ``strict`` was set to false in tsconfig)
- Fix XRFlag component not being applied on startup

## [2.49.0-pre] - 2022-11-24
### Exporter
- Change: Exporter now shows dialogue when trying to export lightmaps with wrong Lightmap encoding

### Engine
- Add: input iterator methods to loop over currently active input pointer ids
- Change: input refactor to work better with touch
- Fix GraphicRaycaster serialization warning
- Fix deserialization bug when Animation clips array is not serialized (exported from blender)
- Fix: remove leftover log in AnimatorController when cloning
- Fix XR flag not correctly restoring state
- Fix reticle not being rendered when XRRig is inside WebARSessionRoot
- Fix Mozilla XR AR overlay (https://github.com/needle-tools/needle-engine-support/issues/81)
- Fix Mozilla XR removing renderer canvas on exit AR (https://github.com/needle-tools/needle-engine-support/issues/115)

## [2.48.0-pre] - 2022-11-23
### Exporter
- Add menu item to copy project info to clipboard (``Needle Engine/Report Bug/Copy Project Info``)
- Change: Reduce max size of default cubemap to 256 (instead of 2048)
- Change: ExportInfo can open folder without explicit workspace in workspace
- Change: remove keep names options in react vite template
- Change: move default project path from ``Projects/`` to ``Needle/``
- Change: remove .quit-ar styles from templates
- Fix: Export skybox in referenced prefabs using minimal size (64px) unless otherwise defined

### Engine
- Add: debug console for better mobile debugging (shows up on error on mobile in local dev environment or when using the ``?console`` query parameter)
- Add: dom element visibility checks and suspend rendering and update loops (if ``this.context.runInBackground`` is false)
- Add: ``this.context.isPaused`` to manually suspend rendering
- Add: ``IComponent.onPausedChanged`` event method which is called when rendering is paused or resumed
- Change: update copy-from-to dev dependency version to fix build error when path contains ``(``
- Change: ``this.context.input`` does now support pointer lock state (properly reports delta)
- Fix: make sure VRButton has the same logic as in three again (regex instead of try-catch)
- Fix: WebXRViewer DOM Overlay bugs when dom overlay element is inside canvas
- Fix: exitAR not being called in some cases when exiting AR
- Fix: ``this.context.domX`` and ``this.context.domY`` when web component is not fullscreen

## [2.47.2-pre] - 2022-11-17
### Exporter
- Add info to log about where to change colorspace from gamma to linear
 
### Engine
- Add: Initial react three fiber components
- Change: OrbitControls made lerp stop distance smaller 
- Change: expose ``*enumerateActions()`` in AnimatorController
- Fix: Flipped custom reflection texture
- Fix: Volume exposure not being applied when no Tonemapping effect was set
- Fix: Volume tonemapping not respecting override state setting
- Fix: ``AudioSource.loop`` not working
- Fix: Collider center being not not applied correctly
- Fix: MeshCollider scale not being applied from object

## [2.47.1-pre] - 2022-11-16
### Exporter
- Bump Engine version and export particle trail material

### Engine
- Add: Particles subemitter support
- Add: Particles inherit velocity support
- Add: Particles size by speed support
- Add: Particles color by speed support
- Add: Particles trail now fadeout properly when "die with particle" is disabled
- Add: Particles circle shape
- Change: button hover now sets cursor to pointer
- Fix: WebXR controller disabling raycast line for hands
- Fix: WebXR hands path when not assigned in Unity
- Fix: Mesh Particles not rendering because of rotation being wrongly applied
- Fix: Mesh particles size in AR
- Fix: Particles color and size lerp between two curves

## [2.47.0-pre] - 2022-11-14
### Exporter
- Change: AxesHelper component now shows axes like in threejs
- Change: bump UnityGLTF version

### Engine
- Add: RemoteSkybox option to control if its set as background and/or environment
- Add: @serializable decorator, @serializeable will be removed in a future version
- Add: getComponent etc methods to IGameObject interface
- Add: Renderer.enable does now set visible state only without affecting the hierarchy or component active state
- Change: Expose Typestore
- Change: Animation componet does loop by default (use the AdditionalAnimationData component to set the default loop setting)
- Fix: WebXR relative hands path in subfolders
- Fix: Rigidbody did not properly detect object position change if the position change was applied a second time at the exact same target position (it worked setting it once and didnt work in subsequent calls - now it does always detect it)

## [2.46.0-pre] - 2022-11-11
### Exporter
- Change: ``Setup scene`` when creating a new camera it sets near clip plane to smaller value than default
- Change: ExportInfo pick directory button now opens last selected directory if it still exists and is in the same Unity project

### Engine
- Add: Particles limit velocity over time
- Add: Particles rotation by speed
- Add: ParticleSystem play, pause, stop and emit(count) methods
- Add: ``WebXR.showRaycastLine`` exposed so it can be disabled from code
- Fix: issues in applying some forces/values for different scaling and worldspace <> localspace scenarios
- Change: raise input events in core method to also allow receiving WebAR mock touch events
- Change: ``Animation.play()`` does not require argument anymore

## [2.45.0-pre] - 2022-11-10
### Exporter
- Add: gzip option to build menu
- Change default build to not gzipped (can be enabled in Unity's Build Window)
- Change: open output directory after building distribution
- Change: bump UnityGLTF dependency
- Fix: glitch project name must not contain spaces

### Engine
- Add: particles emission over distance
- Add: particles can enable trail (settings are not yet applied tho) 
- Add: camera now useses culling mask settings
- Add: particle VelocityOverLife
- Add: particle basic texture sheet animation support
- Change: ensure ``time.deltaTime`` is always > 0 and nevery exactly 0
- Fix: progressbar handle progress event not reporting total file size
- Fix: layer on camera did affect visibility
- Fix: cloning animatorcontrollers in builds did fail because of legacy AnimatorAction name check
- Fix: ``RGBAColor.lerpColors`` did produce wrong alpha value
- Fix: custom shader ``_ZTest`` value is now applied as threejs depthTest function

## [2.44.2-pre] - 2022-11-09
### Exporter
- add: export of particle mesh
- change: bump UnityGLTF dependency
- change cubemap export: make sure the path for flipping Y and not flipping Y applies the same Y rotation

### Engine
- add ``Graphics.copyTexture``
- add ``Renderer.allowProgressiveLoad``
- add ``Gizmos.DrawBox`` and ``DrawBox3``
- add particles burst emission
- add particles color interpolation between two gradients
- fix: reflection probe material caching for when material is being changed at certain times outside of animation loop and cache applied wrong material
- fix: AnimationCurve evaluation when time and keyframe are both exactly 0
- change: reflection probe now requires anchor override
- change: bump threejs dependency 

## [2.44.1-pre] - 2022-11-07
### Exporter
- Fix: serialization error for destroyed component

### Engine
- Add: start adding particle systems support again
- Change: update dependency version to needle gltf-transform-extensions package
- Change: light set to soft shadows now changes renderer shadow mode to ``VSMShadowMap`` (can be disabled by setting ``Light.allowChangingShadowMapType`` to false)
- Fix: WebXR creating AR button when called from script in awake 
- Fix: ``AnimationCurve.evaluate``

## [2.44.0-pre] - 2022-11-05
### Exporter
- Add: ``Create/Typescript`` can now create script files in ``src/scripts`` if the selected file in the ProjectBrowser is not part of an npmdef - it will create a template typscript file with your entered name and open the workspace
- Change: Update component compiler version fixing codegen for e.g. ``new Vector2(1, .5)`` which previously generated wrong C# code trying to assign doubles instead of floats

### Engine
- Add support for deleting all room state by calling ``context.connection.sendDeleteRemoteStateAll()`` (requires backend to update ``@needle-tools/needle-tiny-networking-ws`` to ``^1.1.0-pre``)
- Add Hinge joint
- Add ``Gizmos.DrawLine``, ``DrawRay`` ``DrawWireSphere`` and ``DrawSphere``
- Add: physics Collision Contacts now contain information about ``impulse`` and ``friction``
- Add ``physics.raycastPhysicsFast`` as a first method to raycast against physics colliders, the returning object contains the point in worldspace and the collider. This is the most simplest and thus fastest way to raycast using Rapier. More complex options will follow in future versions.
- Fix joint matrix calculation
- Fix and improve physics Contacts point calculations  
- Fix issue in physics event callbacks where ``onCollisionStay`` and ``onCollisionExit`` would only be called when ``onCollisionEnter`` was defined

## [2.43.0-pre] - 2022-11-04
### Exporter
- Change: Set template body background to black

### Engine
- Add: physics FixedJoint
- Change: CharacterController now rotates with camera
- Change: scaled mesh colliders are now cached
- Change: disable OrbitControls when in XR
- Change: first enabled camera component sets itself as rendering camera if no camera is yet assigned (mainCamera still overrides that)
- Change: package module field now shows to ``src/needle-engine``
- Change: ``Camera.backgroundColor`` assigning Color without alpha sets alpha to 1 now
- Fix: improved missing ``serializable`` detection / warning: now only shows warning for members actually declared in script 
- Fix: wrong light intensity in VR when light is child of WebARSessionRoot [issue 103](https://github.com/needle-tools/needle-engine-support/issues/103) 

## [2.42.0-pre] - 2022-11-02
### Exporter
- Add: explicit shadow bias settings to ``LightShadowData`` component (can be added via Light component button at the bottom of the component)
- Fix ComponentCompiler / CodeWatcher not starting to watch directory when project is not installed yet
- Fix ``CubemapExporter.ConvertCubemapToEquirectTexture`` now using same codepath as skybox export
- Fix ``ExportInfo.Play`` button does not use same code path as Editor Play button

### Engine
- Add ``context.isInAR`` and ``context.isInVR`` properties
- Add physics capsule collider support
- Add basic character controller implementation (experimental)
- Add ``context.input.getMouseWheelDeltaY()``
- Add: SmoothFollow option to restrict following on certain axes only for position
- Add: ``Rigidbody.teleport`` method to properly reset internal state
- Add: load glbs using build hash (appended as ``?v=123``)
- Change: Collision event args now exposes contacts array
- Fix Exit AR (X) button not showing up
- Fix physics collider center offset
- Fix removing colliders and rigidbodies throwing error (when trying to access properties for already removed bodies)
- Fix bug in AnimatorController causing broken animations when the same clip is used in multiple states (caused by ``mixer.uncacheCip``)
- Fix rigidbody friction allowing for physical bodies being transported on e.g. platforms
- Fix ``onTriggerStay`` being invoked with the correct collider argument
- Fix AnimatorController exit time not being used properly
- Fix AnimatorController not checking all possible transitions if one transition did match conditions but could not be made due to exit time setting
- Fix ``Renderer.sharedMaterials`` not handling SkinnedMeshRenderer
- Fix environment blend mode for mozilla XR browser on iOS
- Fix: Camera now removing self from being set as currently rendering in ``onDisable``


## [2.41.0-pre] - 2022-10-28
### Exporter
- Change: enable Auto Reference in Needle Engine asmdef

### Engine
- Add: rapier physics backend and overall improved physics system like constraint support, fixed physics collider updates and synchronization between rendering and physics world or animation of physical bodies 
- Remove: cannon-es
- Add basic mesh collider support
- Add ``@validate`` decorator and ``onValidate`` event method that can be used to automatically get callbacks when marked properties are being written to (for example internally this is used on the Rigidbody to update the physics body when values on the Rigidbody component are being updated)
- Change: assign nested gltf layers
- Change: reworked Rigidbody api
- Fix: allow Draco and KRTX compression on custom hand models
- Fix: applying Unity layers to threejs objects
- Fix: BoxHelper stopped working with SpatialTrigger
- Fix: AR reticle showing up in wrong position with transformed WebARSessionRoot

## [2.40.0-pre] - 2022-10-26
### Exporter
- Add: Warnings when nesting GltfObjects with gltf models that are only copied to the output directory (effectively not re-exported) with prefab overrides
- Add: Animation component can now be configured with random time scale and offset using the additional data component (see "Add AnimationData" button on Animation component)
- Add: nested .gltf assets now copy their dependencies to the output directory
- Change: Refactor deploy to FTP using ScriptableObjects for server settings
- Change: Better compression is only used when explicitly configured by adding a ``TextureCompressionSettings`` component to the GltfObject because it also increases filesize significantly and is not always needed
- Fix: Remove old texture callback that caused textures to be added to a glb twice in some cases

### Engine
- Add: Expose WebXR hand model path
- Add: Animation component can now be configured with random time scale and offset
- Change: allow blocking overlay errors using the ``?noerrors`` query parameter
- Change: don't use Composer for postprocessing in XR (see [issue](https://github.com/needle-tools/needle-engine-support/issues/101)) 
- Change: physics intersections causing NaN's are now reported prominently and physics bodies are removed from physics world as an interim solution, this provides more information about problematic colliders for debugging
- Fix: bug that caused component events for onEnable and onDisable not being called anymore in some cases
- Fix: cases where loading overlay using old project template wouldnt be removed/hidden anymore
- Fix: WebXR hide large hand grab sphere
- Fix: onPointerUp event not firing using WebXR controllers when grabbing an object for the second time
- Fix: GroundProjection can now be removed again
- Fix: Custom shaders exported using builtin RP can now use  _Time property
- Fix: Only create two controllers when in AR on oculus browser
- Fix: BoxHelperComponent can now handle multi-material objects (groups) 

## [2.39.3-pre] - 2022-10-24
### Exporter
- Change: Remove GltfObject component from default Avatar prefab
- Fix: DeployToFTP connection error

### Engine
- Add: warning balloon when unknown components are detected and have been most likely forgot to be installed, linking to npmdef docs 
- Fix: dont show serialization warning for builtin components where specific fields are not deserialized on purpose (since right now the serializer does not check which fields are actually implemented) 

## [2.39.2-pre] - 2022-10-24
### Exporter
- Change: Disable timer logs

### Engine
- Change: AudioSource exposes ``clip`` field
- Change: improve error and messaging overlay
- Change: detect when serialized Object3D and AssetReference are missing ``@serializable`` attribute and show message in overlay
- Change: add WebXR hands path to controllers
- Fix: WebXR controllers now use interactable object when grabbing (instead of hit object previously) which fixes interaction with nested hierarchies in XR and DragControls

## [2.39.1-pre] - 2022-10-23
### Exporter
- Fix: improve generating temporary project with npmdef dependencies
- Fix: avoid attempting to start server twice when project is being generated

## [2.39.0-pre] - 2022-10-23
### Exporter
- Add DeployToFTP component
- Fix automatically installing dependencies to temporary project when the project was already generated from another scene

### Engine
- Change: Renderer ``material`` is now ``sharedMaterial`` to make it more clear for Unity devs that the material is not being cloned when accessed
- Fix: When not specifying any explicit networking backend for glitch deployment it now falls back to the current glitch instance for networking

## [2.38.1-pre] - 2022-10-21
### Exporter
- Add: creating npmdef now automatically creates ``index.ts`` entry point (and adds it to ``main`` in package.json)
- Change: bump UnityGLTF dependency

### Engine
- Add: Screenshare component ``share`` method now takes optional options to configure device and MediaStreamConstraints for starting the stream 
- Fix: WebXR should show EnterVR button when enabled in Unity
- Fix: component ``enable`` boolean wasnt correctly initialized when loaded from gltf
- Fix: Object3D prototype extensions weren't correctly applied anymore
- Fix: Interaction bug when using DragControls with OrbitControls with multitouch

## [2.38.0-pre] - 2022-10-20
### Exporter
- Add: toktx compression extension is now automatically used, can be disabled by adding the ``TextureCompressionSettings`` component to the GltfObject and disabling it
- Change: adjust menu items

### Engine
- Add ``Renderer.mesh`` getter property
- Change: ``Renderer.material`` now returns first entry in ``sharedMaterials`` array so it automatically works in cases where a Renderer is actually a multi-material object
- Change: warn when trying to access components using string name instead of type
- Change: update needle gltf-transform-extensions to 0.6.2
- Fix: remove log from UIRaycastUtil
- Fix: move TypeStore import in builtin engine again to not break cases where ``import @needle-engine`` was never used
- Fix: React3Fiber template and AR overlay container access when using react

## [2.37.1-pre] - 2022-10-19
### Exporter
- Change: allow overriding minimum skybox resolution for root scene (minimum is 64)

### Engine
- Change: unify component access methods, first argument is now always the object with the component type as second argument
- Fix physics collision events throwing caused by refactoring in last version
- Fix loading screen css

## [2.37.0-pre] - 2022-10-19
### Exporter
- Add ``ImageReference`` type: textures exported as ``ImageReference`` will be copied to output assets directory and serialized as filepaths instead of being included in glTF
- Change: Reduce default size of progressive textures (in ``UseProgressiveTextures`` component)
- Change: Update UnityGLTF dependency fixing normal export bug and serializing text in extensions now using UTF8

### Engine
- Change: First pass of reducing circular dependencies
- Change: Update @needle-tools/gltf-transform-extensions version
- Change: Update component compiler to 1.9.0. Changed include:
   * Private and protected methods will now not be emitted anymore
   * ``onEnable/onDisable`` will be emitted as ``OnEnable`` and ``OnDisable`` [issue 93](https://github.com/needle-tools/needle-engine-support/issues/93)
- Change: handle Vector3 prototype extensions
- Fix: issue with UI causing rendering to break when enabling text components during runtime that have not yet been active before
- Fix: OrbitControls LookAtConstraint reference deserialization
- Fix: WebXRController raycasting against UI marked as ``noRaycastTarget`` or in CanvasGroup with disabled ``interactable`` or ``blocksRaycast``

## [2.36.0-pre] - 2022-10-17
### Exporter
- Change: Move Screensharing aspect mode settings into VideoPlayer component (in ``VideoPlayerData``)

### Engine
- Add: start adding support for 2D video overlay mode
- Change: Install threejs from @needle-tools/npm - this removes the requirement to have git installed and should fix a case where pulling the package from github would fail 
- Change: Move Screensharing aspect mode settings into VideoPlayer component
- Change: Move ``InstancingUtils`` into ``engine/engine_instancing.ts``
- Change: BoxCollider now checks if ``attachedRigidBody`` is assigned at start
- Change: Collision now exposes internal cannon data via ``__internalCollision`` property
- Fix: EventSystem now properly unsubscribes WebXRController events

## [2.35.5-pre] - 2022-10-17
### Exporter
- Change: rename ``codegen/exports.ts`` to ``codegen/components.ts``
- Change: ScreenCapture component has explicit VideoPlayer component reference to make it clear how it should be used
 
### Engine
- Add: ScreenCapture has mode for capturing webgl canvas (unfortunately it doesnt seem to work well in Chrome or Firefox yet)
- Change: move threejs prototype extensions into own file and make available to vanilla js builds
- Change: ScreenCapture component has explicit VideoPlayer component reference
- Fix: animating properties on custom shaders

## [2.35.4-pre] - 2022-10-15
### Exporter
- Change: dont automatically run install on referenced npmdefs when performing export
- Fix issue where browser scrollbar would flicker in certain cases when OS resolution was scaled 

### Engine
- Add: start implementing trigger callbacks for ``onTriggerEnter``, ``onTriggerExit`` and ``onTriggerStay``
- Change: ``GameObject.setActive`` now updates ``isActiveAndEnabled`` state and executes ``awake`` and ``onEnable`` calls when the object was activated for the first time (e.g. when instantiating from an previously inactive prefab)
- Change: improve collision callback events for components (``onCollisionEnter``, ``onCollisionExit`` and ``onCollisionStay``)
- Change: this.context.input keycode enums are now strings
- Fix: local dev error overlay now also displays errors that happen before web component is completely loaded (e.g. when script has import error)
- Fix: Rigidbody force is now correctly applied when the component was just instantiated (from inactive prefab) and added to the physics world for the first time
- Fix: DragControls component keyboard events ("space" and "d" for modifying height and rotation)

## [2.35.3-pre] - 2022-10-14
### Exporter
- Change: delete another vite cache
- Change: improve Codewatcher for scripts in ``src/scripts``

## [2.35.2-pre] - 2022-10-14
### Exporter
- Change: delete vite caches before starting server

## [2.35.1-pre] - 2022-10-14
### Exporter
- Change: only serialize used Camera fields
- Change: prevent serializing TextGenerator
- Change: prevent exporting Skybox if no skybox material exists
- Change: prevent installing referenced npmdefs while server is running hopefully fixing some issues wiht vite/chrome where type declarations become unknown
- Fix: loading relative font paths when exported via Asset context menu

### Engine
- Change: Rigidbody now tracks position changes to detect when to update/override simulated physics body
- Fix: loading relative font paths when exported via Asset context menu

## [2.35.0-pre] - 2022-10-13
### Exporter
- Change: make default SyncCam prefab slightly bigger
- Change: log error when ExportInfo GameObject is disabled in the hierarchy

### Engine
- Add: inital ScreenCapture component for sharing screens and camera streams across all connected users
- Add: ``onCollisionEnter``, ``onCollisionStay`` and ``onCollisionExit`` event methods to components

## [2.34.0-pre] - 2022-10-12
### Exporter
- Add temporary support for legacy json pointer format
- Add warning to Build Window when production build is selected but installed toktx version does not match recommended version
- Add warning if web project template does not contain package.json
- Add react template
- Add: allow exporting glbs from selected assets via context menu (previously this only worked in scene hierarchy, it now works also in project window)
- Changed: SpectatorCam improvements, copying main camera settings (background, skybox, near/far plane)
- Changed: improved ExportInfo when selecing web project template
- changed: dont export hidden Cinemachine Volume component
- Changed: update UnityGLTF dependency
- Changed: use source identifier everywhere to resolve absolute uri from relative uris as a first step of loading glbs including dependencies from previously unknown directories
- Fix: when exporting selected glbs with compression all dependent glbs (with nested references) will automatically also be compressed after export
- Fix: Cubemap rotation

### Engine
- Add: Quest 2 passthrough support
- Add: UI Graphic components now support ``raycastTarget`` again
- Add: VideoPlayer now supports ``materialTarget`` option which allows for assigning any renderer in the scene that should be used as a video canvas
- Changed: updated three-mesh-ui dependency version
- Changed: updated needle-gltfTransform extensions package, fixing an issue with passthrough of texture json pointers
- Changed: selecting SpectatorCam now requires click (instead of just listening to pointer up event)
- Fix: Avatars using instanced materials should now update transforms correctly again

## [2.33.0-pre] - 2022-10-10
### Exporter
- Fix: error log caused by unused scene template subasset
- Change: allow exporting ParticleSystem settings
- Change: re-word some unclear warnings, adjust welcome window copy
- Change: dont automatically open output folder after building

### Engine
- Add: Context.removeCamera method
- Add: SpectatorCam allows to follow other users across devices by clicking on respective avatar (e.g. clicking SyncedCam avatar or WebXR avatar, ESC or long press to stop spectating)
- Add: ``Input`` events for pointerdown, pointerup, pointermove and keydown, keyup, keypress. Subscribe via ``this.context.input.addEventListener(InputEvents.pointerdown, evt => {...})`` 
- Change: Default WebXR rig matches Unity forward
- Fix: WebXRController raycast line being rendered as huge line before first world hit
- Fix: SpectatorCam works again
- Fix: ``serializable()`` does now not write undefined values if serialize data is undefined
- Fix: exit VR lighting

## [2.32.0-pre] - 2022-10-07
### Exporter
- Add: toktx warning if toktx version < 4.1 is installed.
- Add: button to download recommended toktx installer to Settings 
- Change: Bump UnityGLTF version
- Change: Builder will install automatically if Needle Engine directory is not found

### Engine
- Add: ``resolutionScaleFactor`` to context
- Fix ``IsLocalNetwork`` regex
- Fix custom shaders failing to render caused by json pointer change
- Change: rename Context ``AROverlayElement`` to ``arOverlayElement``

## [2.31.0-pre] - 2022-10-06
### Exporter
- Add first version of TextureCompressionSettings component which will modify toktx compression settings per texture
- Fix skybox export being broken sometimes
- Fix Vite template update version of vite compression plugin to fix import error
- Change: json pointers now have correct format (e.g. ``/textures/0`` instead ``textures/0``)
- Change: Bump needle glTF transform extensions version

### Engine
- Fix: EventList failing to find target when targeting a Object3D without any components
- Fix: text now showing up when disabling and enabling again after the underlying three-mesh-ui components have been created
- Fix: Builtin sprites not rendering correctly in production builds
- Change: Bump needle glTF transform extensions version
- Change: json pointers now have correct format (e.g. ``/textures/0`` instead ``textures/0``)
- Change: Bump UnityGLTF version

## [2.30.1-pre] - 2022-10-05
### Exporter
- Fix animating ``activeSelf`` on GameObject in canvas hierarchy
- Fix ExportInfo directory picker
- Removed unused dependencies in Vite project template
- Removed wrapper div in Vite project template

### Engine
- Fix animating ``activeSelf`` on GameObject in canvas hierarchy
- Fix SpectatorCam component
- Fix WebXRController raycast line being rendered as huge line before first world hit

## [2.30.0-pre] - 2022-10-05
### Exporter
- Add: experimental AlignmentConstraint and OffsetConstraint
- Fix: font-gen script did use require instead of import
- Change: delete vite cache on server start

### Engine
- Add: experimental AlignmentConstraint and OffsetConstraint
- Remove: MeshCollider script since it is not supported yet
- Change: Camera does now use XRSession environment blend mode to determine if background should be transparent or not.
- Change: WebXR exposes ``IsInVR`` and ``IsInAR``
- Fix: RGBAColor copy alpha fix
- Fix: Avatar mouth shapes in networked environment

## [2.29.1-pre] - 2022-10-04
### Exporter
- Add folder path picker to ExportInfo
- Change message on first installation and when a project does not exist yet
- Change prevent projects being generated in Assets and Packages folders

### Engine
- Change: DropListener file drop event does send whole gltf instead of just the scene

## [2.29.0-pre] - 2022-10-04
### Exporter
- Add: Local error overlay shows in AR
- Add: itchio inspector build type can now be toggled by holding ALT
- Fix: URP 12.1 api change
- Change: Vite template is updated to Vite 3
- Change: Bump UnityGLTF dependency
- Change: Move glTF-transform extension handling into own package, using glTF transform 2 now

### Engine
- Add: allow overriding draco and ktx2 decoders on <needle-engine> web component by setting ``dracoDecoderPath``, ``dracoDecoderType``, ``ktx2DecoderPath`` 
- Add: ``loadstart`` and ``progress`` events to <needle-engine> web component
- Fix rare timeline animation bug where position and rotation of objects would be falsely applied
- Change: update to three v145
- Change: export ``THREE`` to global scope for bundled version

## [2.28.0-pre] - 2022-10-01
### Exporter
- Remove: legacy warning on SyncedCamera script
- Fix: exception during font export or when generating font atlas was aborted
- Change: Export referenced gltf files using relative paths
- Change: Bump runtime engine dependency

### Engine
- Add: make engine code easily accessible from vanilla javascript
- Fix: handle number animation setting component enable where values are interpolated
- Change: Remove internal shadow bias multiplication
- Change: Addressable references are now resolved using relative paths
- Change: Update package json

## [2.27.2-pre] - 2022-09-29
### Exporter
- Bump runtime engine dependency 

### Engine
- Add: Light component shadow settings can not be set/updated at runtime
- Fix: enter XR using GroundProjectedEnv component
- Fix: Light shadows missing when LightShadowData component was not added in Unity (was using wrong shadowResolution)
- Change: dont allow raycasting by default on GroundProjectedEnv sphere

## [2.27.1-pre.1] - 2022-09-29
### Exporter
- Fix compiler flag bug on OSX [issue 76](https://github.com/needle-tools/needle-engine-support/issues/76)

## [2.27.1-pre] - 2022-09-29
### Exporter
- Add: Detect outdated threejs version and automatically run ``npm update three``
- Add: shadow resolution to LightShadowData component
- Add: Warning to GroundProjectedEnvironment inspector when camera far plane is smaller than environment radius

### Engine
- Add: Light exposes shadow resolution

## [2.27.0-pre] - 2022-09-28
### Exporter
- Add RemoteSkybox component to use HDRi images from e.g. polyhaven
- Add GroundProjectedEnv component to use threejs skybox projection 

### Engine
- Add RemoteSkybox component to use HDRi images from e.g. polyhaven
- Add GroundProjectedEnv component to use threejs skybox projection 
- Fix: export ``GameObject`` in ``@needle-tools/engine``

## [2.26.1-pre] - 2022-09-28
### Exporter
- Add LightShadowData component to better control and visualize directional light settings

### Engine
- Add: ``noerrors`` url parameter to hide overlay
- Fix: WebXR avatar rendering may be visually offset due to root transform. Will now reset root transform to identity

## [2.26.0-pre] - 2022-09-28
### Exporter
- Add: tricolor environment light export
- Add: generate exports for all engine components
- Add: export for InputActions (NewInputSystem)

### Engine
- Add: ``@needle-tools/engine`` now exports all components
- Add: environment light from tricolor (used for envlight when set to custom but without custom cubemap assigned)
- Add: show console error on screen for localhost / local dev environment
- Fix: create environment lighting textures from exported colors
- Change: UI InputField expose text
- Change: Bump threejs version to latest (> r144) which also contains USDZExporter PR

## [2.25.2-pre] - 2022-09-26
### Exporter
- Fix collab sandbox scene template, cleanup dependencies
- Fix ShadowCatcher export in Built-in RP
- Fix WebHelper nullreference exception
- Change: remove funding logs, improve log output
- Change: exporting with wrong colorspace is now an error
- Change: Bump UnityGLTF dependency
- Change: add log to Open VSCode workspace

### Engine
- Add: custom shader set ``_ScreenParams``
- Change: DropListener event ``details`` now contains whole gltf file (instead of just scene object)

## [2.25.1-pre] - 2022-09-23
### Exporter
- Bump Engine dependency

### Engine
- Add: AudioSource volume and spatial blending settings can now be set at runtime
- Fix: AudioSource not playing on ``play`` when ``playOnAwake`` is false

## [2.25.0-pre] - 2022-09-23
### Exporter
- Add: automatically include local packages in vscode workspace
- Add: experimental progressive loading of textures
- Fix: Catch ``MissingReferenceException`` in serialization
- Fix: Environment reflection size clamped to 256 for root glb and 64 pixel for referenced glb / asset
- Fix: ShadowCatcher inspector info and handle case without renderer
- Change: ComponentGen types are regenerated when player scriptcount changes

### Engine
- Add: VideoPlayer crossorigin attribute support
- Add: ``debuginstancing`` url parameter flag
- Add: Image handle builtin ``Background`` sprite
- Add: Component now implements EventTargt so you can use ``addEventListener`` etc on every component
- Add: EventList does automatically dispatch event with same name on component. E.g. UnityEvent named ``onClick`` will be dispatched on component as ``on-click``
- Add: experimental progressive loading of textures
- Add: ``WebXR`` exposes ``IsARSupported`` and ``IsVRSupported``
- Fix: remove Ambient Intensity
- Fix: ShadowCatcher material should not write depth

## [2.24.1-pre] - 2022-09-22
### Exporter
- Remove: all scriban templating
- Change: TypeUtils clear cache ond recompile and scene change
- Change: move SyncedCamera into glb in Sandbox template
- Change: Show warning in GltfObject inspector when its disabled in main scene but not marked as editor only since it would still be exported and loaded on startup but most likely not used
- Change: scene template assets use UnityGLTF importer by default
- Change: TypeInfoGenerator for component gen does now prioritize types in ``Needle`` namespace (all codegen types), ``Unity`` types and then everything else (it will also only include types in ``Player`` assemblies)

### Engine
- Fix: SpatialTrigger intersection check when it's not a mesh
- Fix: UnityEvent / EventList argument of ``0`` not being passed to the receiving method
- Fix: Physics rigidbody/collider instantiate calls
- Fix: Physics rigidbody transform changes will now be applied to internal physics body 
- Fix: ``needle-engine.getContext`` now listens to loading finished event namely ``loadfinished``
- Change: cleanup some old code in Rigidbody component

## [2.24.0-pre] - 2022-09-21
### Exporter
- Add: new ``DeployToItch`` component that builds the current project and zips it for uploading to itch.io
- Add: FontGeneration does not try to handle selected font style
- Add: Show ``SmartExport`` dirty state in scene hierarchy (it postfixes the name with a *, similar to how scene dirty state is visualized)
- Add: ``Collect Logs`` now also includes all currently known typescript types in cache
- Remove: legacy ``ScriptEmitter`` and ``TransformEmitter``. Code outside of glb files will not be generated anymore
- Change: Renamed ``Deployment`` to ``DeployToGlitch``
- Change: Set typescript cache to dirty on full export
- Change: automatically run ``npm install`` when opening npmdef workspace
- Change: Bump UnityGLTF dependency to ``1.16.0-pre`` (https://github.com/prefrontalcortex/UnityGLTF/commit/aa19dd2a4f2f3f533888deb47920af6a6b4bf80b)
- Fix: ``Setup Scene`` context menu now sets directional light shadow when creating a light
- Fix: "Project Install Fix" did sometimes fail if an orphan but empty folder was still present in node_modules and ``npm install`` didn't install the missing package again 
- Fix: Exception where FullExport would fail if no ``caches~`` directory exists
- Fix: CodeWatcher threading exception when many typescript files changed (or are added) at once
- Fix: FontGenerator issue where builtin fonts would be unnecessarily re-generated
- Fix: Regression in custom reflection texture export

### Engine
- Add: initial support for ``InputField`` UI (rendering, input handling on desktop, mobile and AR, change and endedit events)
- Add: ``EventList.invoke`` can now handle an arbitrary number of arguments
- Change: lower double click threshold to 200ms instead of 500ms
- Change: runtime font-style does not change font being used in this version. This will temporarely break rich text support.
- Fix: custom shader regression where multiple objects using the same material were not rendered correctly
- Fix: Text sometimes using invalid path
- Fix: Remove unused imports

## [2.23.0-pre] - 2022-09-20
### Exporter
- Add: support for ignoring types commented out using ``//``. For example ``// export class MyScript ...``
- Add: ``Setup Scene`` context menu creates directional light if scene contains no lights
- Add: support for environment light intensity multiplier
- Change: typescache will only be updated on codegen, project change or dependencies changed
- Change: improve font caching and regenerating atlas for better dynamic font asset support

### Engine
- Add basic support for ``CameraDepth`` and ``OpaqueTexture`` (the opaque texture still contains transparent textures in this first version) being used in custom shaders

## [2.22.1-pre] - 2022-09-17
### Exporter
- Fix missing dependency error when serialized depedency in ExportInfo was installed to package.json without the npmdef being present in the project.
- Fix typo in BoxGizmo field name

### Engine
- Improve Animator root motion blending
- Fix SpatialTrigger error when adding both SpatialTrigger as well as SpatialTrigger receiver components to the same object
- AnimatorController can now handle states with empty motion or missing clips

## [2.22.0-pre] - 2022-09-15
### Exporter
- Add: automatic runtime font atlas generation from Unity font assets 
- Change: setup scene menu item does not create grid anymore and setup scene
- Fix: serialization where array of assets that are copied to output directory would fail to export when not all entries of the array were assigned
- Fix: obsolete SRP renderer usage warning in Unity 2021
- Fix: serialize LayerMask as number instead of as ``{ value: <number> }`` object

### Engine
- Add: automatic runtime font atlas generation from Unity font assets 
- Remove: shipped font assets in ``include/fonts``
- Fix: Physics pass custom vector into ``getWorldPosition``, internal vector buffer size increased to 100
- Fix: SpatialTrigger and SpatialTrigger receivers didnt work anymore due to LayerMask serialization

## [2.21.1-pre] - 2022-09-14
### Exporter
- Bump Needle Engine version
- Fix: WebXR default avatar hide hands in AR
- Change: UI disable shadow receiving and casting by default, can be configured via Canvas

### Engine
- Change: UI disable shadow receiving and casting by default, can be configured via Canvas
- Fix: ``gameObject.getComponentInParent`` was making false call to ``getComponentsInParent`` and returning an array instead of a single object
- Fix: light intensity in AR

## [2.21.0-pre] - 2022-09-14
### Exporter
- Remove legacy UnityGLTF export warning
- Fix: add dependencies to Unity package modules (this caused issues when installing in e.g. URP project template)
- Change: will stop running local server before installing new package version
- Change: Bump UnityGLTF version to 1.15.0-pre

### Engine
- Add: first draft of Animator root motion support
- Fix: ``Renderer.sharedMaterials`` assignment bug when GameObject was mesh
- Fix: Buttons set to color transition did not apply transition colors
- Fix: UI textures being flipped
- Fix: UI textures were not stretched across panel but instead being clipped if the aspect ratio didnt match perfectly

## [2.20.0-pre] - 2022-09-12
### Exporter
- Add Timeline AnimationTrack ``SceneOffset`` setting export
- Change: improved ProjectReporter (``Help/Needle Engine/Zip Project``)

### Engine
- Add stencil support to ``Renderer``
- Add timeline ``removeTrackOffset`` support
- Fix timeline animation track offset only being applied to root
- Fix timeline clip offsets not being applied when no track for e.g. rotation or translation exists

## [2.19.0-pre] - 2022-09-11
### Exporter
- Add ShadowCatcher enum for toggling between additive and ShadowMask
- Add initial support for exporting URP RenderObject Stencil settings
- Add support for animating ``activeSelf`` and ``enabled``
- Change: improved ProjectReporter (``Help/Needle Engine/Zip Project``)
- Bump: UnityGLTF dependency

### Engine
- Add initial UI anchoring support
- Add initial support for URP RenderObject Stencil via ``NEEDLE_render_objects`` extension

## [2.18.3-pre] - 2022-09-09
### Exporter
- Bump runtime engine dependency

### Engine
- Fix UI transform handling for [issue 42](https://github.com/needle-tools/needle-engine-support/issues/42) and [issue 30](https://github.com/needle-tools/needle-engine-support/issues/30)
- Fix AudioSource not restarting to play at onEnable when ``playOnAwake`` is true (this is the default behaviour in Unity)

## [2.18.2-pre] - 2022-09-09
### Exporter
- Change default skybox size to 256
- Fix hash cache directory not existing in certain cases

### Engine
- Fix RGBAColor not implementing copy which caused alpha to be set to 0 (this caused ``Camera.backgroundColor`` to not work properly)

## [2.18.1-pre.1] - 2022-09-08
### Exporter
- Fix gitignore not found
- Fix hash cache directory not existing in certain cases

## [2.18.0-pre] - 2022-09-08
### Exporter
- Add ``Zip Project`` in ``Help/Needle Engine/Zip Project`` that will collect required project assets and data and bundle it

## [2.17.3-pre] - 2022-09-07
### Exporter
- Add auto fix if .gitignore file is missing
- Add menu item to only build production dist with last exported files (without re-exporting scene)
- Fix dependency change event causing error when project does not exist yet / on creating a new project
- Fix updating asset hash in cache directory when exporting

### Engine
- Add support to set OrbitControls camera position immediately

## [2.17.2-pre] - 2022-09-07
### Exporter
- Bump Engine dependency version

### Engine
- Fix EventList invocation not using deserialized method arguments

## [2.17.1-pre] - 2022-09-07
### Exporter
- Fix DirectoryNotFound errors caused by dependency report and depdendency cache
- Fix writing dependency hash if exported from play buttons (instead of save) and hash file doesnt exist yet

## [2.17.0-pre] - 2022-09-07
### Exporter
- Add export on dependency change and skip exporting unchanged assets
- Add ``EmbedSkybox`` toggle on GltfObject component
- Add simple skybox export size heuristic when no texture size is explictly defined (256 for prefab skybox, 1024 for scene skybox)
- Add debug information log which allows for basic understanding of why files / assets were exported
- Remove old material export code
- Change: clamp skybox size to 8px
- Fix skybox texture settings when override for Needle Engine is disabled, fallback is now to default max size and size
- Fix exceptions in ``Collect Logs`` method
- Fix Glitch ``Deploy`` button to only enable if deployment folder contains any files

### Engine
- Add ``context`` to ``StateMachineBehaviour``
- Fix ``StateMachineBehaviour`` binding (event functions were called with wrong binding)

## [2.16.0-pre.3] - 2022-09-06
### Exporter
- Fix compiler error when no URP is installed in project

### Engine
- Fix deserialization error when data is null or undefined

## [2.16.0-pre.1] - 2022-09-05
### Exporter
- Add EXR extension and export (HDR skybox)
- Add initial tonemapping and exposure support (start exporting Volume profiles)
- Add AR ShadowCatcher
- Add automatic re-export of current scene if referenced asset changes
- Fix potential nullref in BugReporter
- Change: add additional info to test npm installed call 
- Change server process name to make it more clear that it's the local development server process 
- Change: bumb UnityGLTF dependency

### Engine
- Add initial tonemapping and exposure support
- Add AR shadow catcher
- Fix objects parented to camera appear behind camera
- Fix reticle showing and never disappearing when no WebARSessionRoot is in scene
- Fix WebARSessionRoot when on same gameobject as WebXR component
- Fix deserialization of ``@serializable(Material)`` producing a new default instance in certain cases
- Fix ``OrbitControls`` enable when called from UI button event
- Fix EventList / UnityEvent calls to properties (e.g. ``MyComponent.enable = true`` works now from UnityEvent)

## [2.15.1-pre] - 2022-09-02
### Exporter
- Add skybox export using texture importer settings (for Needle Engine platform) if you use a custom cubemap
- Bump ShaderGraph dependency
- Fix compiler error in Unity 2021
- Change automatically flag component compiler typemap to be regenerated if any generated C# has compiler errors

### Engine
- Change: ``OrbitControls.setTarget`` does now lerp by default. Use method parameter ``immediate`` to change it immediately
- Change: bump component compiler dependency to ``1.8.0``

## [2.14.2-pre] - 2022-09-01
### Exporter
- Bump runtime dependency
- Fix settings window not showing settings when nodejs/npm is not found

### Engine
- Fix EventList serialization for cross-glb references
- Fix AnimatorController transition from state without animation

## [2.14.0-pre] - 2022-09-01
### Exporter
- Add: mark GltfObjects in scene hierarchy (hierarchy elements that will be exported as gltf/glb files)
- Add FAT32 formatting check and warning
- Fix: setup scene
- Fix: try improving ComponentGenerator component to watch script/src changes more reliably

### Engine
- Fix: skybox/camera background on exit AR
- Change: AnimatorController can now contain empty states
- Change: Expose ``Animator.Play`` transition duration

## [2.13.1-pre] - 2022-08-31
### Exporter
- Fix UnityEvent argument serialization 
- Fix generic UnityEvent serialization 

## [2.13.0-pre] - 2022-08-31
### Exporter
- Add report bug menu items to collect project info and logs
- Remove legacy ResourceProvider code

### Engine
- Improved RectTransform animation support and canvas element positioning
- Fix ``Animator.Play``
- Change: Expose ``AnimatorController.FindState(name)`` 

## [2.12.1-pre] - 2022-08-29
### Exporter
- Fix UnityEvent referencing GameObject

## [2.12.0-pre] - 2022-08-29
### Exporter
- Add UI to gltf export
- Add better logging for Glitch deployment to existing sites that were not remixed from Needle template and dont expose required deployment api
- Add AnimatorController support for any state transitions

### Engine
- Add UI to gltf export
- Add button animation transition support for triggers ``Normal``, ``Highlighted`` and ``Pressed``

## [2.11.0-pre] - 2022-08-26
### Exporter
- Add Linux support
- Add additional npm search paths for OSX and Linux to the settings menu
- Add ShaderGraph dependency to fix UnityGLTF import errors for projects in 2021.x
- Fix exporting with Animation Preview enabled

### Engine
- Add ``Canvas.renderOnTop`` option
- Fix ``OrbitControls`` changing focus/moving when interacting with the UI
- Fix nullref in AnimatorController with empty state

## [2.10.0-pre] - 2022-08-25
### Exporter
- Add export for ``Renderer.allowOcclusionWhenDynamic``
- Fix issue in persistent asset export where gameObjects would be serialized when referenced from within an asset

### Engine
- Add export for ``Renderer.allowOcclusionWhenDynamic``
- Fix: bug in ``@serializable`` type assignment for inherited classes with multiple members with same name but different serialized types
- Change: ``GameObject.findObjectOfType`` now also accepts an object as a search root

## [2.9.5-pre] - 2022-08-25
### Exporter
- OSX: add homebrew search path for npm

### Engine
- Fix canvas button breaking orbit controls [issue #4](https://github.com/needle-tools/needle-engine-support/issues/4)

## [2.9.4-pre.1] - 2022-08-23
### Exporter
- Fix glitch component for private projects

## [2.9.3-pre] - 2022-08-23
### Exporter
- Fix passing UnityGLTF export settings to exporter
- Fix old docs link
- Fix timeline extension export in certain cases, ensure it runs before component extension export
- Update minimal template

### Engine
- Fix SyncedRoom to not append room parameter multiple times

## [2.9.2-pre] - 2022-08-22
### Exporter
- Fix: Minor illegal path error
- Change: ExportInfoEditor ``Open`` button to open exporter package
- Change: ExportInfoEditor clear versions cache when clicking update button

### Engine
- Add: Timeline AudioTrack nullcheck when audio file is missing
- Fix: AnimatorController error when behaviours are undefined
- Change StateMachineBehaviour methods to be lowercase

## [2.9.1-pre] - 2022-08-22
### Exporter
- Fix build errors and compilation warnings

## [2.9.0-pre] - 2022-08-22
### Exporter
- Add initial StateMachineBehaviour support with "OnStateEnter", "OnStateUpdate" and "OnStateExit"
- Update UnityGLTF dependency
- Fix: prevent scene templates from cloning assets even tho cloning was disabled
- Fix: ifdef for URP

### Engine
- Add initial StateMachineBehaviour support with "OnStateEnter", "OnStateUpdate" and "OnStateExit"
- Fix input raycast position calculation for scrolled content

## [2.8.2-pre] - 2022-08-22
### Exporter
- Fix exporting relative path when building distribution: audio path did produce absolute path because the file was not yet copied
- Fix bundle registry performance bug causing a complete reload / recreation of FileSystemWatchers
- Fix texture pointer remapping in gltf-transform opaque extension
- Change: skip texture-transform for textures starting with "Lightmap" for now until we can configure this properly 

### Engine
- Fix texture pointer remapping in gltf-transform opaque extension
- Change: skip texture-transform for textures starting with "Lightmap" for now until we can configure this properly 

## [2.8.1-pre] - 2022-08-19
### Exporter
- Fix rare timeline export issue where timeline seems to have cached wrong data and needs to be evaluated once
- Update sharpziplip dependency

## [2.8.0-pre] - 2022-08-18
### Exporter
- Add new template with new beautiful models
- Change start server with ip by default from Play button too
- Fix Glitch deployment inspector swapping warning messages when project does not exist
- Fix certificate error spam when port is blocked by another server

### Engine
- Add scale to instantiation sync messages
- Fix ``BoxHelper``
- Fix AR reticle being not visible when ``XRRig`` is child of ``WebARSessionRoot`` component
- Fix exception in ``DragControls`` when dragged object was deleted while dragging

## [2.7.0-pre] - 2022-08-18
### Exporter
- Change name of ``KHR_webgl_extension`` to ``NEEDLE_webgl_extension``
- Change start server to use IP by default (ALT to open with localhost)
- Fix export cull for ShaderGraph with ``RenderFace`` option (instead of ``TwoSided`` toggle)

### Engine
- Change name of ``KHR_webgl_extension`` to ``NEEDLE_webgl_extension``
- Change: dont write depth for custom shader set to transparent 
- Deprecate and disable ``AssetDatabase``

## [2.6.1-pre] - 2022-08-17
### Exporter
- Add codegen buttons to npmdef inspector (regenerate components, regenerate C# typesmap)
- Add DefaultAvatar and SyncedCam default prefab references
- Change: allow cancelling process task when process does not exist anymore
- Change: ExportInfo inspector cleanup and wording
- Fix Timeline Preview on export (disable and enable temporarely)
- Fix constant names
- Fix XR buttons in project templates
- Fix VideoPlayer for iOS
- Fix Editor Only hierarchy icon
- Fix order of menu items and cleanup/remove old items
- Fix timeline clip offset when not offset should be applied
- Fix project templates due to renamed web component
- Fix and improve setup scene menu item

### Engine
- Add ``Mathf.MoveTowards``
- Change: rename ``needle-tiny`` webcomponent to ``needle-engine``
- Fix ordering issue in needle web component when codegen.js is executed too late

## [2.5.0-pre] - 2022-08-16
### Exporter
- Add ShaderGraph double sided support
- Add ShaderGraph transparent support
- Add SyncedCamera prefab support
- Remove legacy shader export code

### Engine
- Add SyncedCamera prefab/AssetReference support
- Add TypeArray support for ``serializable`` to provide multiple possible deserialization types for one field (e.g. ``serializable([Object3D, AssetReference])`` to try to deserialize a type as Object3D first and then as AssetReference)

## [2.4.1-pre] - 2022-08-15
### Exporter
- Add error message when trying to export compressed gltf from selection but engine is not installed.

### Engine
- Add event callbacks for Gltf loading: ``BeforeLoad`` (use to register custom extensions), ``AfterLoaded`` (to receive loaded gltf), ``FinishedSetup`` (called after components have been created)

## [2.4.0-pre] - 2022-08-15
### Exporter
- Add minimal analytics for new projects and installations
- Add log to feedback form
- Fix minor context menu typo

## [2.3.0-pre] - 2022-08-14
### Exporter
- Add warning to Camera component when background type is solid color and alpha is set to 0
- Add ``CameraARData`` component to override AR background alpha
- Change Glitch deployment secret to only show secret in plain text when ALT is pressed and mouse is hovered over password field 
- Fix ``ExportInfo`` editor "(local)" postfix for installed version text at the bottom of the inspector
- Fix scene templates build command
- Fix Glitch project name paste to not wrongly show "Project does not exist"

### Engine
- Fix AnimatorController exit state
- Fix AR camera background alpha to be fully transparent by default 

## [2.2.1-pre] - 2022-08-12
### Exporter
- Add: Export context menu to scene hierarchy GameObjects
- Fix: Multi column icon rendering in ProjectBrowser
- Fix: Builder now waits for installation finish
- Fix: Copy include command does not log to console anymore
- Fix: Invalid glb filepaths
- Fix: URP light shadow bias exported from RendererAsset (when setup in light)

### Engine
- Fix: light shadow bias

## [2.2.0-pre] - 2022-08-11
### Exporter
- Add: Problem solver "Fix" button
- Change: Use Glitch Api to detect if a project exists and show it in inspector
- Change: Typescript template file
- Change: Disable codegen for immutable packages
- Change: Improved problem solver messages
- Change: Renamed package.json scripts
- Change: Run "copy files" script on build (to e.g. load pre-packed gltf files at runtime when project was never built before)
- Fix: Logged Editor GUI errors on export
- Fix: gltf-transform packing for referenced textures via pointers
- Fix: Don't try to export animations for "EditorOnly" objects in timeline
- Fix: ComponentLink does now npmdef VSCode workspace

### Engine
- Add ``@needle-tools/engine`` to be used as import for "most used" apis and functions
- Change: remove obsolete ``Renderer.materialProperties``
- Fix: ``NEEDLE_persistent_assets`` extension is now valid format (change from array to object)

## [2.1.1-pre] - 2022-08-09
### Exporter
- Add Option to Settings to disable automatic project fixes
- Fix Build Window

## [2.1.0-pre] - 2022-08-08
### Exporter
- Add fixes to automatically update previous projects

## [2.0.0-pre] - 2022-08-08
### Exporter
- Renamed package
- Add: npmdef pre-build callback to run installation if any of the dependencies is not installed
- Add: Glitch Deployment inspector hold ALT to toggle build type (development or production)

### Engine
- Renamed package

## [1.28.0-pre] - 2022-08-08
### Exporter
- Add: Custom Shader vertex color export
- Add: NestedGltf objects and components do now have a stable guid
- Fix: NestedGltf transfrom

### Engine
- Fix: NestedGltf transform

## [1.27.2-pre] - 2022-08-06
### Exporter
- Remove: Scene Inspector experimental scene asset assignment
- Change: update templates
- Change: Component guid generator file ending check to make it work for other file types as well
- Change: add logo to scenes in project hierarchy with Needle Engine setup

### Engine
- Remove: Duplicateable animation time offset hack
- Change: GameObjectData extension properly await assigning values
- Change: NestedGltf instantiate using guid
- Change: ``instantiate`` does now again create guids for three Objects too

## [1.27.1-pre] - 2022-08-05
### Exporter
- Change: always export nested GlbObjects
- Change: update scene templates
- Change: Spectator camera component now requires camera component

### Engine
- Add: NestedGltf ``listenToProgress`` method
- Add: Allow changing Renderer lightmap at runtime
- Fix: Environment lighting when set to flat or gradient (instead of skybox)
- Fix: ``this.gameObject.getComponentInChildren`` - was internally calling wrong method
- Fix: Spectator camera, requires Camera component in glb now

## [1.27.0-pre] - 2022-08-03
### Exporter
- Add: warning if lightmap baking is currently in progress
- Add: support to export multiple selected objects
- Change: Audio clips are being exported relative to glb now (instead of relative to root) to make context menu export work, runtime needs to resolve the path relative to glb
- Fix: Selected object export collect types from ExportInfo

### Engine
- Add: Animator.keepAnimatorStateOnDisable, defaults to false as in Unity so start state is entered on enable again
- Add: warning if different types with the same name are registered
- Add: timeline track ``onMutedChanged`` callback
- Change: PlayableDirector expose audio tracks
- Change: BoxCollider and SphereCollider being added to the physics scene just once
- Change: try catch around physics step


## [1.26.0-pre] - 2022-08-01
### Exporter
- Add: open component compiler menu option to open Npm package site
- Add: feedback form url menu item
- Add: support for nested ``GltfObject``
- Add: support to copy gltf files in your hierarchy to the output directory instead of running export process again (e.g. a ``.glb`` file that is already compressed will just be copied and not be exported again. Adding components or changing values in the inspector won't have any effect in that case)
- Change: Don't export skybox for nested gltfs
- Change: bump component compiler dependency to ``1.7.2``
- Change: Unity progress name changed when running Needle Engine server process
- Remove: legacy export options on ``GltfObject``, components will now always be exported inside gltf extension
- Fix: delete empty folder when creating a new scene from a scene template 
- Fix: CodeWatcher error caused by repaint call from background thread
- Fix: Don't serialize in-memory scene paths in settings (when creating scenes from scene templates)
- Fix: Array serialization of e.g. AudioClip[] to produce Array<string> (because audio clips will be copied to the output directory and be serialized as strings which did previously not work in arrays or lists)
- Fix: component link opens workspace again
- Fix: scene save on scene change does not trigger a new export/build anymore

### Engine
- Add: Addressable download progress is now observeable
- Add: Addressable preload support, allows to load raw bytes without actually building any components
- Add: PlayableDirector exposes tracks / clips
- Change: modify default engine loading progress bar to be used from user code
- Change: add option to Instantiate call to keep world position (set ``keepWorldPosition`` in ``InstantiateOptions`` object that you can pass into instantiate)
- Change: light uses shadow bias from Unity
- Fix: instancing requiring worldmatrix update being not properly processed
- Fix: Duplicatable world position being off (using ``keepWorldPosition``)
- Fix: ``Animation`` component, it does allow to use one main clip only now, for more complex setups please use AnimationController or Timeline
- Fix: ``SyncedRoom`` room connection on enter WebXR
- Fix: WebXR avatar loading

## [1.25.0-pre] - 2022-07-27
### Exporter
- Add: Send upload size in header to Glitch to detect if the instance has enough free space
- Add: menu item to export selected object in hierarchy as gltf or glb
- Add: Timeline animation track infinite track export (when a animation track does not use TimelineClips)
- Add: ``AnimatorData`` component to expose and support random animator speed properties and random start clip offsets to easily randomize scenes using animators with the same AnimatorController on multiple GameObjects
- Fix: npmdef import, sometimes npmdefs in a project were not registered/detected properly which led to problems with installing dependencies
- Fix: script import file-gen does not produce invalid javascript if a type is present in multiple packages
- Change: improved error log message when animation export requires ``KHR_animation_pointer``
- Change: server starts using ``localhost`` url by default and can be opened by ip directly by holding ALT (this removes the security warning shown by browsers when opening by ip that does not have a security certificate which is only necessary if you want to open on another device like quest or phone. It can still be opened by ip and is logged in he console if desired)

### Engine
- Change: bump component compiler dependency to ``1.7.1``
- Change: ``context.mainCameraComponent`` is now of type ``Camera``
- Fix: timeline control track
- Fix: timeline animation track post extrapolation
- Fix: custom shader does not fail when scene uses object with transmission (additional render pass)

## [1.24.2-pre] - 2022-07-22
### Exporter
- Add: Deployment component now also shows info in inspector when upload is in process 
- Fix: cancel deploy when build fails
- Fix: better process handling on OSX

## [1.24.1-pre] - 2022-07-22
### Exporter
- Change: ``Remix on Glitch`` button does not immediately remix the glitch template and open the remixed site

## [1.24.0-pre] - 2022-07-21
### Exporter
- Add: glitch deploy auto key request and assignment. You now only need to paste the glitch project name when remixed and the deployment key will be requested and stored automatically (once after remix)
- Fix: process output log on OSX
- Fix: process watcher should now use far less CPU
- Change: move internal publish code into separate package
### Engine
- add loading bar and show loading state text

## [1.23.1-pre] - 2022-07-20
- Fix check if toktx is installed
- Fix: disable build buttons in Build Settings Window and Deployment component when build is currently running
- Fix: dont allow running multiple upload processes at once
- engine: add using ambient light settings (Intensity Multiplier) exported from Unity

## [1.23.0-pre] - 2022-07-18
- Update UnityGLTF dependency version
- Fix packing texture references on empty gameobjects
- Fix npmdef problem factory for needle.engine and three packages
- Add help urls to our components
- engine: fix nullref in registering texture

## [1.22.0-pre.2] - 2022-07-18
- Refactor problem validation and fixing providing better feedback messages
- Add: log of component that is not installed to runtime project but used in scene
- Change: Glitch deploy buttons
- Change: Build Settings window with new icons

## [1.21.0-pre] - 2022-07-15
- Add: moving npmdef in project should now automatically resolve path in package.json (if npmdef name didnt change too)
- Add: ``Show in explorer`` to scene asset context menu
- Add: warn when component is used in scene/gltf that is not installed to current runtime project
- engine: remove legacy file
- engine: add basic implementation of ``Context.destroy``
- engine: fix ``<needle-tiny>`` src attribute
- engine: add implictly creating camera with orbit controls when loaded glb doesnt contain any (e.g. via src) 

## [1.20.3-pre.1] - 2022-07-13
- Fix exception in ComponentCompiler editor
- Fix type list for codegen including display and unavailable types

## [1.20.2-pre.2] - 2022-07-12
- Add warning to Typescript component link (in inspector) when component on GameObject is not the codegen one (e.g. multiple components with the same name exist in the project)
- Change component compiler to not show ``install`` button when package is not installed to project
- Change recreate codewatchers on editor focus
- engine: fix dont apply lightmaps to unlit materials
- engine: remove log in PlayableDirector
- engine: add support to override (not automatically create) WebXR buttons 

## [1.20.1-pre] - 2022-07-11
- Fix TypesGenerator log
- Fix ExportInfo editor when installing
- Fix: ComponentCompiler serialize path relative to project
- Fix Inspector typescript link
- Fix AnimatorController serialization in persistent asset extension
- Fix AnimatorController serialization of transition conditions
- Add more verbose output for reason why project is not being installed, visible when pressing ALT
- Fix process output logs to show more logs
- Update component compiler default version to 1.6.2
- engine: Fix AnimatorController finding clip when cloned via ``AssetReference.instantiate``
- engine: Fix deep clone array type
- engine: Fix PlayableDirectory binding when cloned via ``AssetReference.instantiate``

## [1.20.0-pre] - 2022-07-10
- Add info to ExportInfo component when project is temporary (in Library folder)
- Add ``Open in commandline`` context menu to ExportInfo component
- Add generating types.json for component generator to remove need to specify C# types explicitly via annotations
- Add context menu to ComponentGenerator component version text to open changelog
- Change: hold ALT to perform clean install when clicking install button
- Fix: KHR_animation_pointer now works in production builds
- engine: add VideoPlayer using ``AudioOutputMode.None``
- engine: fix VideoPlayer waiting for input before playing video with audio (unmuted) and being loaded lazily

## [1.19.0-pre] - 2022-07-07
- Add: automatically import npmdef package if npmdef package.json contains (existing) ``main`` file
- Add: Timeline serializer does not automatically create asset model from custom track assets for fields marked with ``[SerializeField]`` attribute
- Change: PlayableDirector allow custom tracks without output binding
- engine: Add ``getComponent`` etc methods to THREE.Object3D prototype so we can use it like in Unity: ``this.gameObject.getComponent...``
- engine: Change ``Duplictable`` serialization

## [1.18.0-pre] - 2022-07-06
- Add temp projects support: projects are temp projects when in Unity Library
- Change prevent creating project in Temp/ directory because Unity deletes content of symdir directories
- Change ExportInfo update button to open Package Manager by default (hold ALT to install without packman)
- Change starting processes with ``timeout`` instead of ``pause``
- Change: try install npmdef dependency when package.json is not found in node_modules
- Fix ComponentGenerator path selection
- Fix warning from UnityGLTF api change
- Fix codegen import of register_types on very first build
- engine: Fix networking localhost detection
- engine: update component generator package version (supporting now CODEGEN_START and END sections as well as //@ifdef for fields)

## [1.17.0-pre] - 2022-07-06
- Add mathematics #ifdef
- Change NpmDef importer to enable GUI to be usable in immutable package
- Change Move modules out of this package
- Fix ``Start Server`` killing own server again
- Fix error when searching typescript workspace in wrong directory
- Change lightmap extension to be object
- engine: change lightmap extension to be object

## [1.16.0-pre] - 2022-07-06
- Add DeviceFlag component
- Add build stats log to successfully built log printing info about file sizes
- Add warning for when Unity returns missing/null lightmap
- Add VideoPlayer ``isPlaying`` that actually checks if video is currently playing
- Add ObjectField for npmdef files to SceneEditor
- Fix BuildTarget for 2022
- Fix serializing UnityEvent without any listeners
- Fix seriailizing component ``enable`` state
- Fix skybox in production builds
- Improve VideoTrack editor preview
- Improve glitch deploy error message when project name is wrong
- Update gltf-transform versions in project templates
- Update UnityGLTF method names for compatibility with 1.10.0-pre
- engine: Add DeviceFlag component
- engine: Fix VideoPlayer loop and playback speed
- engine: Improve VideoTrack sync

## [1.15.0-pre] - 2022-07-04
- add VideoTrack export
- add Spline export
- fix ComponentCompiler finding path automatically
- fix Unity.Mathematics float2, float3, float4 serialization
- change: ExportInfo shows version during installation 
- engine: fix ``enabled`` not being always assigned
- engine: fix react-three-fiber component setting camera
- engine: add support for custom timeline track
- engine: add VideoTrack npmdef

## [1.14.3-pre] - 2022-07-01
- Add: installation progress now tracks and warns on installations taking longer than 5 minutes
- engine: Change; PlayableDirector Wrap.None now stops/resets timeline on end
- engine: Change; PlayableDirector now stops on disabling  

## [1.14.2-pre] - 2022-07-01
- Update UnityGltf dependency
- engine: fix timeline clip offsets and hold
- engine: fix timeline animationtrack support for post-extrapolation (hold, loop, pingpong)

## [1.14.1-pre] - 2022-06-30
- Fix: exception in code watcher when creating new npmdef
- Fix: issue when deleting npmdef
- engine: improve timeline clip offsets

## [1.14.0-pre] - 2022-06-30
- Add: export timeline AnimationTrack TrackOffset
- engine: Improved timeline clip- and track offset (ongoing)
- engine: Change assigning all serialized properties by default again (instead of require ``@allProperties`` decorator)
- engine: Change; deprecate ``@allProperties`` and ``@strict`` decorators

## [1.13.2-pre] - 2022-06-29
- Fix Playmode override
- Fix: Dispose code watcher on npmdef rebuild
- Add button to open npmdef directory in commandline
- Change: keep commandline open on error
- engine: add methods for unsubscribing to EventList and make constructor args optional
- engine: change camera to not change transform anymore

## [1.13.1-pre] - 2022-06-28
- Fix support for Unity 2022.1
- engine: add support for transparent rendering using camera background alpha

## [1.13.0-pre] - 2022-06-27
- Add: transform gizmo component
- Change: component generator for npmdef is not required anymore
- Change: component gen runs in background now
- Fix: typescript drag drop adding component twice in some cases
- engine: update component gen package dependency
- engine: fix redundant camera creation when exported in GLTF
- engine: fix orbit controls focus lerp, stops now on input

## [1.12.1-pre] - 2022-06-25
- Override PlayMode in sub-scene
- engine: lightmaps encoding fix
- engine: directional light direction fix 

## [1.12.0-pre] - 2022-06-25
- SceneAsset: add buttons to open vscode workspace and start server

## [1.11.1-pre] - 2022-06-25
- AnimatorController: can now be re-used on multiple objects
- Add support for exporting current scene to glb, export scene on save when used in running server
- Fix: issue that caused multi-select in hierarchy being changed
- Add glb and gltf hot-reload option to vite.config in template
- Add context menu to ``ExportInfo`` to update vite.config from template
- engine: animator controler can handle multiple target animators
- engine: fix WebXR being a child of WebARSessionRoot
- engine: improve Camera, OrbitControls, Lights OnEnable/Disable behaviour
- engine: add ``Input.getKeyPressed()``

## [1.10.0-pre] - 2022-06-23
- Support exporting multiple lightmaps
- Fix custom reflection being saved to ``Assets/Reflection.exr``
- engine: fix light error "can't add object to self" when re-enabled
- engine: remove extension log
- engine: log missing info when UnityEvent has not target (or method not found)
- engine: use lightmap index for supporting multiple lightmaps

## [1.10.0-pre] - 2022-06-23
- Support exporting multiple lightmaps
- Fix custom reflection being saved to ``Assets/Reflection.exr``
- engine: fix light error "can't add object to self" when re-enabled
- engine: remove extension log
- engine: log missing info when UnityEvent has not target (or method not found)
- engine: use lightmap index for supporting multiple lightmaps

## [1.9.0-pre] - 2022-06-23
- Initial support for exporting SceneAssets 
- GridHelper improved gizmo
- engine: Camera dont set skybox when in XR
- engine: dont add lights to scene if baked

## [1.8.1-pre] - 2022-06-22
- Automatically install referenced npmdef packages
- Refactor IBuildCallbackReceiver to be async
- Remove producing resouces.glb
- engine: fix threejs dependency pointer

## [1.8.0-pre.1] - 2022-06-22
- Add project info inspector to scene asset
- Add custom context menu to scene asset containing three export projects
- Export lightmaps and skybox as part of extension
- Known issue: production build skybox is not working correctly yet
- Fix dragdrop typescript attempting to add non-component-types to objects
- Allow overriding threejs version in project
- Bump UnityGLTF dependency
- engine: ``<needle-tiny>`` added awaitable ``getContext()`` (waits for scene being loaded to be used in external js)
- engine: fix finding main camera warning
- engine: add ``SourceIdentifier`` to components to be used to get gltf specific data (e.g. lightmaps shipped per gltf)
- engine: persistent asset resolve fix
- engine: update three dependency to support khr_pointer
- engine: remove custom khr_pointer extension
- engine: fix WebARSessionRoot exported in gltf
- engine: smaller AR reticle

## [1.7.0-pre] - 2022-06-17
- Component generator inspector: add foldout and currently installed version
- Npmdef: fix register_type when no types are in npmdef (previously it would only update the file if any type was found)
- Npmdef: importer now deletes codegen directory when completely empty
- Export: referenced prefabs dont require GltfObject component anymore
- engine: create new GltfLoader per loading request
- engine: fix bug in core which could lead to scripts being registered multiple times
- engine: Added SyncedRoom auto rejoin option (to handle disconnection by server due to window inactivity)
- engine: guid resolving first in loaded gltf and retry in whole scene on fail
- engine: fix nullref in DropListener
- engine: register main camera before first awake

## [1.6.0-pre.1] - 2022-06-15
- fix serializing components implementing IEnumerable (e.g. Animation component)
- update UnityGLTF dependency
- engine: add ``GameObject.getOrAddComponent``
- engine: ``OrbitControl`` exposing controlled object
- engine: ``getWorldPosition`` now uses buffer of cached vector3's instead of only one
- engine: add ``AvatarMarker`` to synced camera (also allows to easily attach ``PlayerColor``)
- engine: fix ``Animation`` component when using khr_pointer extension
- engine: ``VideoPlayer`` expose current time
- engine: fix ``Animator.runtimeController`` serialization
- engine: make ``SyncedRoom.tryJoinRoom`` public

## [1.5.1-pre] - 2022-06-13
- Generate components from js files
- Fix compiler error in 2022
- Improve component generator editor watchlist
- Serialize dictionary as object with key[] value[] lists
- Prevent running exporter while editor is building
- Remove empty folder triggering warning
- Fix component generator running multiple times per file when file was saved multiple times.

## [1.5.0-pre] - 2022-06-12
- Add ``Create/Typescript`` context menu
- Improved npmdef and typescript UX
- Improved component codegen: does now also delete generated components when typescript file or class will be deleted
- Component gen produces stable guid (generated from type name)

## [1.4.0-pre] - 2022-06-11
- Bumb UnityGLTF dependency to 1.8.0-pre
- Add typescript editor integration to NpmDef importer: typescript files are now being displayed in project browser with look and feel of native Unity C# components. They also show a link to the matching Unity C# component.
- Fix PathUtil error
- Fix register-types generator deleting imports for modules that are not installed in current project

## [1.3.4-pre] - 2022-06-10
- Custom shader: start supporting export for Unity 2022.1
- Custom shader: basic default texture support
- engine: allow ``@serializeable`` taking abstract types
- engine: add ``Renderer.sharedMaterials`` support

## [1.3.3-pre] - 2022-06-09
- engine: move log behind debug flag
- engine: improved serialization property assignment respecting getter only properties
- engine: add optional serialization callbacks to ``ISerializable``
- engine: default to only assign declared properties

## [1.3.2-pre] - 2022-06-09
- update UnityGLTF dependency to 1.7.0-pre
- add google drive module (wip)
- project gen: fix path with spaces
- ExportInfo: fix dependency list for npmdef (for Unity 2022)
- set types dirty before building
- engine: downloading dropped file shows minimal preview box
- engine: ``DropListener`` can use localhost
- engine: ``SyncedRoom`` avoid reload due to room parameter
- engine: ``LODGroup`` instantiate workaround
- engine: improve deserialization supporting multiple type levels

## [1.3.1-pre.2] - 2022-05-30
- minor url parsing fix
- engine: change md5 hashing package
- engine: file upload logs proper server error

## [1.3.1-pre.1] - 2022-05-30
- Check if toktx is installed for production build
- Lightmap export: treat wrong quality setting as error
- engine: disable light in gltf if mode is baked
- engine: use tiny starter as default networking backend
- engine: synced file init fix for resolving references
- engine: allow removing of gen.js completely
- engine: expose ``Camera.buildCamera`` for core allowing to use blender camera
- engine: on filedrop only add drag control if none is found

## [1.3.1-pre] - 2022-05-27
- Improved ``ComponentGenerator`` inspector UX
- Add inspector extension for ``AdditionalComponentData<>`` implementations
- Update vite template index.html and index.scriban
- engine: fix networked flatbuffer state not being stored
- engine: make ``src`` on ``<needle-tiny>`` web component optional
- engine: ``src`` can now point to glb or gltf directly
- engine: fix ``Raycaster`` registration
- engine: add ``GameObject.destroySynced``
- engine: add ``context.setCurrentCamera``
- engine: make ``DropListener`` to EventTarget
- engine: make ``DropListener`` accept explicit backend url

## [1.3.0-pre.1] - 2022-05-26
- NPM Definition importer show package name
- PackageUtils: add indent
- MenuItem "Setup Scene" adds ``ComponentGenerator``
- Added minor warnings and disabled menu items for non existing project
- Fix gltf transform textures output when used in custom shaders only
- Fix ``ExportShader`` asset label for gltf extension

## [1.3.0-pre] - 2022-05-25
- Add ``ExportShader`` asset label to mark shader or material for export
- Add output folder path to ``IBuildDistCallbackReceiver`` interface
- Add button to NpmDef importer to re-generate all typescript components
- Add ``IAdditionalComponentData`` and ``AdditionalComponentData`` to easily emit additional data for another component
- engine: fix ``VideoPlayer`` being hidden, play automatically muted until interaction
- engine: added helpers to update window history
- engine: fix setting custom shader ``Vector4`` property

## [1.2.0-pre.4] - 2022-05-25
- Fix project validator local path check
- remove ``@no-check```(instead should add node_modules as baseUrl in tsconfig)
- fix ``Animation`` component serialization
- engine: fix tsc error in ``Animation`` component
- engine: fix ``Animation`` component assigning animations for GameObject again
- engine: fix ``Animation`` calling play before awake
- engine: ``AnimatorController`` handle missing motion (not assigned in Unity)
- engine: ``AnimatorController.IsInTransition()`` fix

## [1.2.0-pre.1] - 2022-05-20
- Disable separate installation of ``npmdef`` on build again as it would cause problems with react being bundled twice
- Add resolve for react and react-fiber to template vite.config
- Adding ``@no-check`` to react component as a temporary build fix solution
- Make template ``floor.fbx`` readable
- engine: minor tsc issues fixed

## [1.2.0-pre] - 2022-05-20
- Add initial react-three-fiber template
- Vite template: cleanup dependencies and add http2 memory workaround
- Dont show dependencies list in ``ExportInfo`` component when project does not exist yet
- Creating new npmdef with default ``.gitignore`` and catch IOException
- Building with referenced but uninstalled npmdef will now attempt to install those automatically
- engine: add ``isManagedExternally`` if renderer is not owned (e.g. when using react-fiber)

## [1.1.0-pre.6] - 2022-05-19
- Add resolve module for peer dependencies (for ``npmdef``) to vite project template
- Various NullReferenceException fixes
- Easily display and edit ``npmdef`` dependencies in ``ExportInfo`` component
- Add problem detection and run auto resolve for some of those problems (e.g. uninstalled dependency)
- engine: add basic support for ``stopEventPropagation`` (to make e.g. ``DragControls`` camera control agnostic in preparation of react support)

## [1.1.0-pre.5] - 2022-05-19
- ``Clean install`` does now delete node_modules and package-lock
- Mark types dirty after installation to fix missing types on first time install
- Fix ``npmdef`` registration on first project load

## [1.1.0-pre.2] - 2022-05-18
- improved ``NpmDef`` support

## [1.1.0-pre] - 2022-05-17
- fix ``EventList`` outside of gltf
- fix ``EventList`` without any function assigned (``No Function`` in Unity)
- fix minimal template gizmo icon copy
- start implementing ``NpmDef`` support allowing for modular project setup.
- engine: support changing ``WebARSessionRoot.arScale`` changing at runtime

## [1.0.0-pre.31] - 2022-05-12
- engine: fix webx avatar instantiate
- engine: stop input preventing key event defaults

## [1.0.0-pre.30] - 2022-05-12
- replace glb in collab sandbox template with fbx
- minor change in ``ComponentGenerator`` log
- add update info and button to ``ExportInfo`` component
- engine: log error if ``instantiate`` is called with false parent
- engine: fix instantiate with correct ``AnimatorController`` cloning

## [1.0.0-pre.29] - 2022-05-11
- engine: fix ``@syncField()``
- engine: fix ``AssetReference.instantiate`` and ``AssetReference.instantiateSynced`` parenting
- engine: improve ``PlayerSync`` and ``PlayerState``

## [1.0.0-pre.28] - 2022-05-11
- Move ``PlayerState`` and ``PlayerSync`` to experimental components
- Add TypeUtils to get all known typescript components
- Add docs to ``SyncTransform`` component
- Add support for ``UnityEvent.off`` state
- engine: prepend three canvas to the web element instead of appending
- engine: ``SyncedRoom`` logs warning when disconnected
- engine: internal networking does not attempt to reconnect on connection closed
- engine: internal networking now empties user list when disconnected from room
- engine: ``GameObject.instantiate`` does not always generate new guid to support cases where e.g. ``SyncTransform`` is on cloned object and requires unique id
- engine: ``syncedInstantiate`` add fallback to ``Context.Current`` when missing
- engine: ``EventList`` refactored to use list of ``CallInfo`` objects internally instead of plain function array to more easily attach meta info like ``UnityEvent.off`` 
- engine: add ``GameObject.instantiateSynced``

## [1.0.0-pre.27] - 2022-05-10
- add directory check to ``ComponentGenerator``
- parse glitch url in ``Networking.localhost``
- engine: fix font path
- engine: add ``debugnewscripts`` url parameter
- engine: start adding simplifcation to automatic instance creation + sync per player
- engine: allow InstantiateOptions in ``GameObject.instantiate`` to be inlined as e.g. ``{ position: ... }``
- engine: add ``AvatarMarker`` creation and destroy events
- engine: fix networking message buffering

## [1.0.0-pre.26] - 2022-05-10
- Fix js emitter writing guid for glTF root which caused guid to be present on two objects and thus resolved references for gltf root were wrong
- Improved ``SyncedRoom`` and ``Networking`` components tooltips and info
- Improved ``SyncedCam`` reference assignment - warn if asset is assigned instead of scene reference
- Build error fix
- Added versions to ``ExportInfo`` editor and context menu to quickly access package.jsons and changelogs

## [1.0.0-pre.25] - 2022-05-08
- Unity 2022 enter PlayMode fix for broken skybox when invoked from play button or ``[InitializeOnLoad]``
- Unity 2022 minor warning / obsolete fixes
- remove GltFast toggle in ``GltfObject`` as currently not supported/used
- fix build error
- rename and update scene templates
- engine: ``SpatialTrigger`` is serializable
- engine: fix ``DragControls`` offset when using without ground
- engine: fix ``WebXRController`` interaction with UI ``Button.onClick``

## [1.0.0-pre.24] - 2022-05-04
- fix ifdef in template component
- allow disabling component gen component
- fix exporting asset: check if it is root
- fix ``InputAction`` locking export
- engine: fix gltf extension not awaiting dependencies
- engine: fix persistent asset @serializable check for arrays
- engine: add ``setWorldScale``
- engine: fix ``instantiate`` setting position
- engine: ``AssetReference`` does now create new instance in ``instantiate`` call
- engine: add awaitable ``delay`` util method
- engine: fix scripts being active when loaded in gltf but never added to scene
- engine: minimal support for mocking pointer input
- engine: emit minimal down and up input events when in AR

## [1.0.0-pre.23] - 2022-05-03
- show warning in ``^2021`` that templating is currently not supported
- clean install now asks to stop running servers before running
- engine: improved default loading element
- play button does now ask to create a project if none exits

## [1.0.0-pre.22] - 2022-05-02
- lightmaps fixed
- glitch upload shows time estimate
- deployment build error fix
- json pointer resolve
- improved auto install
- started basic ``SpriteRenderer`` support
- basic ``AnimationCurve`` support
- fixed ``PlayerColor``
- fixed ``persistent_assets`` and ``serializeable`` conflict
- basic export of references to components in root prefab

## [1.0.0-pre.21] - 2022-04-30
- cleanup ``WebXR`` and ``WebXRSync``
- Play button does not also trigger installation and setup when necessary
- Fixed addressables export
- Added doc links to ``ComponentGenerator`` and updated urls in welcome window.
- ``Deployment.GlitchModel`` does now support Undo

## [1.0.0-pre.20] - 2022-04-29
- add internal publish button
- dont emit ``khr_techniques_webgl`` extension when not exporting custom shaders
- fix environment light export
- use newtonsoft converters when serializing additional data
- add ``Open Server`` button to ``ExportInfo`` component
- ``component-compiler`` logs command and log output file

## [1.0.0-pre.18] - 2022-04-27
- refactor extension serialization to use Newtonsoft

## [1.0.0-pre.11] - 2022-04-22
- initial release