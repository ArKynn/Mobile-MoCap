# Servidor Python

O servidor, é composto por um único ficheiro *server.py* para o seu funcionamento. É responsável por criar um servidor, receber mensagens enviadas e escrever essas mesmas num ficheiro devidamente identificado. Este servidor permite guardar as informações captadas pelo programa num único ficheiro, permitindo fazer uma análise externa dos dados obtidos.

## server.py

Este ficheiro é responsável pelo funcionamento completo do servidor e define as seguintes funções:

* **main**: inicializa o servidor e dá ao mesmo o método handle_websocket para este chamar assim que e enquanto a conexão se realiza e mantém.
* **Handle_message**: primeiro transforma o bytes recebidos pela mensagem de volta em floats e de seguida abre e escreve no ficheiro log o recebido.
* **Handle_websocket**: para cada mensagem recebida, chama o método handle_message.

Além destas funções, o ficheiro define as seguintes instruções a realizar assim que é corrido
* Define o local e nome do ficheiro onde gravar  as mensagens recebidas;
* Encontra e escreve na consola o IP do wifi conectado;
* Corre a função main.
