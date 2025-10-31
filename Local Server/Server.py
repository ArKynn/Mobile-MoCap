import asyncio
import websockets
import socket

hostname = socket.gethostname()
IpAddr = socket.gethostbyname(hostname)
print(IpAddr)


async def handle_websocket(websocket):
        # route and handle messages for duration of websocket connection
        async for message in websocket:
            await handle_message(message),


async def handle_message(message):
    print(message)


async def main():
    async with websockets.serve(
            handle_websocket,
            host=IpAddr,
            port=12348,
    ):
        await asyncio.Future()


asyncio.run(main())