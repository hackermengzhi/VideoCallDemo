<div align="center">

# 📹 VideoCallDemo

**A lightweight, room-based video calling demo built with ASP.NET Core SignalR and WebRTC.**

中文多人视频通话示例，支持最多三人加入同一房间，提供主画面切换、画中画和通话控制。

</div>

---

## ✨ Features

- **Room-based calls** — join with a shared room ID; up to three participants per room.
- **Peer-to-peer media** — WebRTC carries audio and video between browsers.
- **SignalR signaling** — exchanges offers, answers, and ICE candidates through the server.
- **Flexible video layout** — switch the main view between participants and local preview.
- **Call controls** — mute the microphone, toggle the camera, switch front/rear cameras, and hang up.
- **Connection stats** — displays basic frame-rate and bitrate information.

## 🧰 Tech stack

| Layer | Technology |
| --- | --- |
| Signaling server | ASP.NET Core 10, SignalR |
| Media transport | WebRTC (`RTCPeerConnection`, `getUserMedia`) |
| Client | Plain HTML, CSS, and JavaScript |
| SignalR client | Microsoft SignalR JavaScript client (vendored in `wwwroot/`) |

## 🚀 Run locally

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0), then run:

```bash
git clone https://github.com/hackermengzhi/VideoCallDemo.git
cd VideoCallDemo
dotnet run
```

Open the local URL printed by ASP.NET Core in two or three supported browsers/devices. Enter the same room ID on each device.

> Camera and microphone access requires a secure context. `localhost` is treated as secure by modern browsers; when testing from other devices, serve the app over HTTPS and allow camera/microphone permissions.

## 🧭 How it works

1. A browser requests camera and microphone access.
2. It joins a room through the `/hub` SignalR endpoint.
3. Peers exchange WebRTC session descriptions and ICE candidates through SignalR.
4. Audio and video flow directly between peers when network conditions allow.

The server keeps room membership in memory. Restarting the server clears all rooms.

## 🌐 Network configuration

The demo includes Google's public STUN server for basic connectivity discovery. STUN alone cannot establish every connection, especially across restrictive NATs or firewalls. For reliable real-world use, configure a TURN server that you operate and protect its credentials outside source control.

This is a demonstration project, not a production-ready conferencing service. It has no user authentication, persistent room state, or production TURN configuration. The sample limits each room to three participants.

## 📁 Project layout

```text
.
├── Program.cs                 # ASP.NET Core app and SignalR endpoint
├── hubs/VideoHub.cs           # Room membership and signaling relay
└── wwwroot/
    ├── index.html             # Call UI and WebRTC client
    └── signalr.min.js         # SignalR browser client
```

## 📄 License

No license has been added yet. Unless a license is added, the source is shared publicly for viewing, but standard copyright restrictions still apply.

---

<div align="center">

Made as a compact starting point for experimenting with browser-based video calls.

</div>
