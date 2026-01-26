# Unity Project Overview

## Cena Main

A única cena presente no projeto, chamada Main, contém os seguintes objetos mais pertinentes para o funcionamento do programa:

### OutputCanvas e UICanvas

Estes objeto incorporam os seguintes objetos de UI:

* WebCam Output é uma simples imagem vazia. A textura desta imagem é mais tarde substituída por uma obtida através da câmera do dispositivo, mostrando ao utilizador o que o programa está a ver.
* Black Screen é uma simples imagem preta que ofusca o ecrã quando nenhuma imagem é mostrada na WebCam Output.
* SettingsMenu: Contém os seguintes objetos de UI:
  * CameraButton: Alterna a câmera a ser lida entre as várias disponíveis pelo dispositivo, chamando a função *NextWebcamDevice* da classe *WebCamInput*;
  * IP Input é um objeto Input Field, que guarda o IP lá escrito, mais tarde lido pelo programa.
  * ServerButton: envia o texto escrito em IP Input para o objeto *WebsocketClient* de forma a este realizar a ligação ao servidor, via o método TryServerConnection.
  * Pose Save Button chama o método *StartPoseSave* da classe *UIController*, que inicia o processo de guardar a pose atual do utilizador.
* SettingsButton: Liga e desliga o elemento SettingsMenu.
* CountdownTimer: Temporizador geral.
* PoseSimilarity: Mostra a pontuação atual calculada pela classe *PoseSimilarityComparer*.
* ServerButton: Chama o método *StartServerLogCountdown*.
	
### WebCamInput
*WebCamInput* é o objeto responsável pela leitura da câmera. Este objeto têm um único componente do tipo *WebCamInput*. Esta classe lê a câmera e, tanto escreve a imagem numa textura, substitui a presente no objeto *WebCamOutput*, como envia-a para o objeto *Visualizer*.

Este componente apresenta os seguintes parâmetros públicos:
* WebCamResolution define a resolução a usar na textura criada e enviada;

### Visualizer

*Visualizer* é o objeto responsável por enviar a textura obtida pelo objeto *WebCamInput* a um objeto gerado internamente do tipo *BlazePoseDetecter*, como por analisar os dados obtidos por esse objeto e construir o esqueleto de pontos e ligações obtidos. 
Tem dois componentes, o primeiro um componente do tipo *PointLandmarkVisualizer* , uma versão modificada da original presente no projeto *BlazePoseBarracuda*.

Este componente apresenta os seguintes parâmetros públicos:
* **LandmarkPrefab** define que prefab usar na criação dos pontos do esqueleto;
* **LineRendererPrefab** define que prefab usar na criação das ligações entre pontos do esqueleto;
* **Image** define qual a imagem de UI usada para mostrar ao utilizador a imagem captada pela câmera e enviada ao programa.
* **Visualizer Smoothing Points** define quantos pontos a usar no efeito de smoothing. Este valor pode ser modificado pelo utilizador através do Smoothing Controller.
* **Max Visualizer Smoothing Points** indica o máximo de pontos permitidos para este efeito.

O segundo componente é do tipo *PoseSimilarityComparer*. Não apresenta nenhum parâmetro público, mas é responsável por avaliar o quão similar a pose atual e a pose guardada, esta segunda internamente obtida pelo *PoseLandmarkVisualizer*.

### WebsocketClient
*WebsocketClient* é o objeto responsável por estabelecer a comunicação com o servidor remoto, recolher e comprimir a informação a enviar e fazer esse envio. Tem um único componente do tipo *WebSocketClient*.

Este componente apresenta os seguintes parâmetros públicos:
* **Port** define qual porta usar na conexão com o servidor;
* **InputField** define qual objeto deste tipo recolher o IP necessário na conexão com o servidor

## Prefabs

Este projeto utiliza os seguintes objetos pré-definidos para um correto funcionamento:

### Point
Este objeto representa um ponto do esqueleto usado para a representação visual da pose do utilizador. É instanciado um Point por ponto detectado pelo programa.

### LineRenderer
Este objeto representa uma ligação entre pontos do esqueleto usado para a representação visual da pose do utilizador. É instanciado um LineRenderer para cada ligação entre pontos, definida pelo programa para a construção visual da pose.

[Back](../README.md)