import asyncio
import websockets
import socket
import os
import datetime
import struct

filePath = os.path.join(os.path.join(os.path.abspath(os.getcwd()),"logs") , str(datetime.datetime.now().strftime("%Y_%m_%d-%H_%M_%S").__add__(".txt")))
print(filePath)

hostname = socket.gethostname()
IpAddr = socket.gethostbyname(hostname)
print(IpAddr)


async def handle_websocket(websocket):
        # route and handle messages for duration of websocket connection
        async for message in websocket:
            await handle_message(message),


async def handle_message(message):
    with open(filePath, "a") as log:
        if len(message) == 8:
            [x,y] = struct.unpack("@2f", message)
            print(int(x),y, file=log)

        elif len(message) == 16:
            [x,y,z,w] = struct.unpack("@4f", message)
            print(x,y,z,w, file=log)

async def main():
    try:
        async with websockets.serve(
                handle_websocket,
                host=IpAddr,
                port=80,
        ):
            await asyncio.Future()

    except websockets.exceptions.ConnectionClosedOK:
        print("Client closed...")



asyncio.run(main())