# Demo Walkthrough

This is the order I follow when I record the demo video and when I present. Each step says what I do in the headset, what the viewer should see, and what I say about it.

## Controls

| Action | Button |
|---|---|
| Grab or hold | Grip (middle finger) |
| Use a held object, click a UI button with the ray | Trigger (index finger) |
| Press a UI button by touch | Push the index fingertip into the button |
| Smooth turn | Left thumbstick |
| Snap turn (45 degrees, pull back for 180) | Right thumbstick |
| Show the teleport ray | Touch either thumbstick, or hold X or A |
| Teleport | While the teleport ray shows, aim at a rug, mat or floor, then press and release grip |
| Room menu (reset, travel, close) | B |

## Part 1: Eseosa's Room (the Create with VR course scene)

1. **Start.** The app opens in Eseosa's Room, fading in from black, standing on the Entry Mat and facing the welcome panel. I read the panel, then press OK with the ray and trigger to show ray clicking.
2. **Settings.** I open the welcome panel again by pressing B and closing the menu, or I show the gear first. I tick and untick Snap Turning and Smooth Turning to show the turning changes, and I explain Distance Grab.
3. **Hands.** I hold my hands up to the camera, squeeze grip to curl the lower three fingers, pull trigger to curl the index finger, and press both to make a fist. I explain that the controller input drives an Animator blend tree.
4. **Locomotion.** I turn with both sticks, then teleport onto the purple rug (a Teleportation Area, land anywhere) and onto a round mat (a Teleportation Anchor, land on a fixed spot facing a set direction).
5. **Grabbing and throwing.** I pick up the tennis ball near the dining table, throw it at the floor so it bounces, then grab the racket by its handle and hit the ball.
6. **Activation.** I pick up the TV remote and pull the trigger: it clicks, the light turns red and the TV plays. I pull again to pause.
7. **Sockets.** I take a hat off the hook, put it on my head, look in the mirror, then hang it back. I try to put the ball on the hook to show it is refused.
8. **Wall clock.** I look at the analog clock and the digital clock on the living room wall and compare them with the real time on my phone.
9. **Comfort and lighting.** I point out the fade on load, the baked lighting and the soft shadows of things I hold.
10. **Travel.** I press B, then the teal Space Station button. The screen fades and the VR Room loads.

## Part 2: The VR Room (space station)

1. **Start.** The room fades in, I stand on the Start Mat and the welcome panel tells me what to do. I poke Start with my fingertip to show touch input.
2. **Locomotion.** I teleport across the main room floor and into the storage room.
3. **Grabbing.** I pick up the mug, a small box and the origami crane, and throw a box.
4. **Physics door.** I walk to the glass door, grip the handle and slowly pull the door open, showing it follows my hand. I let go and it swings gently shut. I open it the other way by pushing from the other side.
5. **Sockets.** I put a chess piece on the board square, then try the mug there to show it is refused. I take the flashlight out of its dock, show a chess piece will not fit the dock, and put the flashlight back.
6. **Room menu.** I press B: I return to the Start Mat and the Room Menu opens. I press Reset Room to show every object returns to where it started.
7. **Travel back.** I press B, then Eseosa's Room, to show navigation works in both directions without restarting. I grab something there to prove the hands and interactions still work after the switch.

## The door handle

The door has one handle that passes through the glass, so it can be grabbed from either side. Grab whichever side is facing me. There is no button to press. I hold grip and move my hand, and the door follows. When I let go, a gentle spring in the hinge pulls the door back to closed.

## What I explain while presenting

1. **Scene setup.** Each scene has the same top level groups (XR, LIGHTING, STATIC, DYNAMIC, INTERFACE). Both scenes use one XR Rig prefab, so the controls are identical.
2. **Interaction components.** XR Grab Interactable for grabbing, XR Socket Interactor for sockets, Near Far Interactor and Poke Interactor for hands and UI, XR Ray Interactor for teleporting, Tracked Device Graphic Raycaster on every canvas.
3. **Door.** The door leaf has a Rigidbody and a Hinge Joint around its vertical edge with limits of 100 degrees each way and a light closing spring. The handle has its own Rigidbody, a Fixed Joint to the door and an XR Grab Interactable set to Velocity Tracking, so my hand pushes the door with physics instead of moving it directly. The door is on its own physics layer so it never catches on its own frame.
4. **Socket matching.** A socket only accepts objects that share one of its Interaction Layers. The hook and head sockets use Hats, the board uses Chess and the dock uses Flashlight. Everything I can pick up also has Default so my hands can still grab it.
5. **Wall clock.** ClockTicker reads DateTime.Now every frame and raises a Ticked event. AnalogClockListener turns the second hand 6 degrees per second, the minute hand 6 degrees per minute and the hour hand 30 degrees per hour. DigitalClockListener rewrites the text once a second.
6. **Animated hands.** ControllerInputReader reads the grip and trigger values and raises GripChanged and TriggerChanged. HandAnimationListener passes them to the Animator, whose 2D blend tree mixes the Open, Grip, Trigger and Fist poses.
7. **Navigation.** TravelButton raises TravelEvents.Requested with a scene name. TravelFadeListener fades to black and TravelLoadListener loads the scene. Every scene has its own XR Rig, Event System and Input Action Manager, so input works straight away after the switch.
8. **Teleportation.** It needs a Teleportation Provider on the rig, an interactor whose interaction layer matches the surface (Raycasts), and a surface with a collider and a Teleportation Area or Anchor. An Area lets me land anywhere on it. An Anchor always puts me on one point and can turn me to face a set direction.
