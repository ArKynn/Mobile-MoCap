import asyncio
import websockets
import socket
import os
import datetime

logCreated = False,
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
        log.write(message)

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