# Introdução

Mobile Mocap é um projeto desenvolvido na plataforma Unity Engine para dispositivos
Android. Esta aplicação recolhe imagens captadas pela câmera do dispositivo, procura pela
presença de um corpo humano na imagem, analisa a posição do mesmo e mostra os
resultados, construindo um boneco composto por vários pontos detectados e interconectados,
que representa a pose atual do utilizador.

O projeto suporta o uso de um servidor python, disponibilizado à parte que, quando
conectado com a aplicação, recolhe e guarda os dados por detrás da análise feita, gravados
em ficheiros prontos a usar.

Este projeto foi construído por cima do projeto já existente [BlazePoseBarracuda](https://github.com/creativeIKEP/BlazePoseBarracuda) e utiliza
o projeto adicional [WebSocketSharp](https://github.com/sta/websocket-sharp) nas funcionalidades relacionadas ao servidor. O repositório deste projeto pode ser encontrado neste [link](https://github.com/ArKynn/Mobile-MoCap).

[Back](../README.md)