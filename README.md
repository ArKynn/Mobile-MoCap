# Mobile Mocap (Unity + Mobile Pose Estimation)

Mobile Mocap is a Unity-based Android application for **markerless motion capture** using a mobile device camera.  
It detects a human body in real time, estimates its pose using ML-based landmark detection, and reconstructs a visual skeleton inside Unity.

The system can also connect to a **Python server** to stream and log pose landmark data for external analysis.

This project builds on:

- BlazePoseBarracuda (Unity implementation of MediaPipe Pose)
- MediaPipe Pose (Google)
- WebSocketSharp (client–server communication)


## ✨ Features

- Real-time body landmark detection on Android
- Skeleton visualization inside Unity
- Pose smoothing
- Pose saving
- Pose similarity comparison
- WebSocket streaming of landmark data
- External Python logging server
- Validation experiments against VICON motion capture

## 🧠 System Overview

Mobile Camera → Unity → BlazePose Detector → Landmark Skeleton

↓

Pose Similarity

↓

WebSocket → Python Server → Log File

## 📂 Documentation

Detailed documentation is available in `/Documention`:

| Topic | File |
|------|------|
| Introduction | [01-introduction.md](Documentation/01-introduction.md) |
| Landmark Detection | [02-landmark-detection.md](Documentation/02-landmark-detection.md) |
| Unity Architecture | [03-unity-project-overview.md](Documentation/03-unity-project-overview.md) |
| Internal C# Classes | [04-internal-csharp-classes.md](Documentation/04-internal-csharp-classes.md) |
| External C# Classes | [05-external-csharp-classes.md](Documentation/05-external-csharp-classes.md) |
| Python Server | [06-python-server.md](Documentation/06-python-server.md) |
| VICON Validation | [07-vicon-validation.md](Documentation/07-vicon-validation.md) |

---

## 🔧 Requirements

- Unity (Android build support)
- Android device with camera
- Python 3 (for server)
- GPU capable of running Barracuda ML inference

---

## 🧪 Validation

The system was experimentally compared against VICON motion capture for upper and lower limb movements at different orientations.  
See `docs/09-vicon-validation.md`.

---

## 📡 Server Usage

A Python WebSocket server is included to log landmark data. See `docs/08-python-server.md`.

---

## 📜 License

Depends on licenses of integrated projects:
- BlazePoseBarracuda
- MediaPipe
- WebSocketSharp
