# External C# Classes

## BlazePoseDetecter
Responsável por controlar o modelo ML, de enviar a imagem para análise e de disponibilizar os dados obtidos ao resto do programa. Adicionalmente também realiza a rotação da imagem obtida pela câmera, caso esta esteja rotacionada. Esta classe define os seguintes métodos essenciais para o seu funcionamento:

* **ProcessImage**: Recebe a imagem obtida pela câmera, a textura onde mostrar a imagem e o ângulo de rotação da imagem recebida. Contém um conjunto de instruções para o GPU, incluindo:
Rotação da imagem obtida, para corrigir quaisquer rotações que o dispositivo possa ter pré definido na câmera; Previsão da pose segundo a imagem enviada.

Esta classe também disponibiliza dois métodos, *GetPoseLandmark* e *GetPoseWorldLandmark*, onde são disponibilizados os pontos obtidos. O primeiro conjunto guarda as posições relativamente à imagem, não disponibilizando a terceira coordenada de profundidade, e o segundo relativamente ao espaço 3D.

## Websocket
Responsável por controlar a ligação e comunicação com o servidor.

O método *Connect* tenta realizar a conexão entre cliente e servidor, usando o URL previamente enviado via contructor.
