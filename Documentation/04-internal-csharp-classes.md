# Internal C# classes

## BodyLandmarks
*BodyLandmarks* é uma classe static com duas listas públicas chamadas *PoseLandmarks* e *PoseLandmarkPairs*. 
A primeira enumera cada ponto detectado pela aplicação e a segunda guarda cada ligação entre pontos, necessárias para a criação do esqueleto simulado. Esta classe dá acesso destas informações a outras classes e permite fazer modificações aos pontos a criar e ligações a realizar de forma fácil.

Adicionalmente, disponibiliza o metodo *SortPoseLandmarks*, que ordena um conjunto de PoseLandmarks, de forma a estarem na ordem correta.

### Landmark
Landmark é a classe que representa e atualiza os pontos do esqueleto e as suas ligações. Esta classe define os seguintes métodos essenciais para o seu funcionamento:
* **UpdateValues**: Usa um Vector4 que recebe para atualizar este ponto. Chama os metodos *AddSmoothingPoint* e *UpdatePointSmooth* ou *UpdatePoint* consoante se for para usar o efeito de Smoothing. Por final atualiza o material do ponto, representando o quão visível este está para o sistema.
* **UpdateLineRenderers**: Atualiza cada linerenderer com os dados mais recentemente recebidos.
* **UpdateSingleLineRenderer**: Permite atualizar manualmente um linerenderer em especifico, especificando o seu index e o seu visibility score.
* **AddSmoothingPoint**: Adiciona o ponto recebido à lista de pontos usados para o efeito de Smoothing, removendo o mais antigo se não houver espaço.
* **UpdatePointSmooth**: calcula a média das posições e visibilidades dos pontos da lista para este efeito e atualiza os valores a usar pelo método *UpdatePositionVectorNanCheck*;
* **UpdatePoint**: atualiza a visibilidade do ponto diretamente, e a posição através do método *UpdatePositionVectorNanCheck*;
* **UpdatePositionVectorNanCheck**: Faz um pequeno check de erro antes de atualizar diretamente a posição do ponto.
* **InitializeSmoothing**: Cria uma lista de pontos a usar para o efeito de smoothing.
* **SetNext**: adiciona um ponto à lista dos pontos ligados e instancia um objeto LineRenderer para fazer essa representação.

### PointLandmarkVisualizer
*PointLandmarkVisualizer* é a classe responsável por criar um esqueleto que simula a pose do utilizador através dos dados obtidos pela classe *BlazePoseDetecter*, de guardar a pose num instante e de controlar o efeito de Smoothing. Esta classe define os seguintes métodos essenciais para o seu funcionamento:
* **Start**: Prepara o funcionamento desta classe, cria um novo *BlazePoseDetecter*, a classe onde a detecção da pose do utilizador é feita, e chama o método *InitializePose*;
* **LateUpdate**: envia as informações de câmera recebidas por *WebCamInput* ao *BlazePoseDetecter*, através do método *ProcessImage*, e de chamar o método *UpdatePoints* da classe *Pose*.
* **InitializePose**: inicia a criação de uma nova pose. Primeiro cria um novo GameObject, dá-lhe um novo componente do tipo *Pose* e chama o método *Init* da classe Pose de forma a preparar a criação de uma nova pose, com as informações que lhe forem dadas.
* **GetLandmarkPointData**: prepara e envia as informações de cada ponto de uma pose, chamada por *WebsocketClient*, objeto responsável pela conexão entre a aplicação e o servidor remoto.
* **SaveCurrentPose**: Cria uma cópia exata do objeto que guarda e que representa a pose detectada, efetivamente gravando a pose nesse instante. De seguida chama o metodo *StartComparer*, iniciando o comparador de poses e *UpdateSavedPointsAlpha*.

### WebCamInput
WebCamInput é a classe responsável por inicializar e atualizar a câmera ativa, e de enviar o que é lido pela câmera ao resto do programa. Esta classe define os seguintes métodos essenciais para o seu funcionamento:

* **Start**: Prepara o funcionamento desta classe e inicializa a câmera, usando o método *InitializeWebcam*.
* **InitializeWebcam**: inicia o dispositivo e a textura a usar e força a primeira atualização da câmera pelo método *UpdateWebcam*.
* **UpdateWebcam**: Começa a leitura da câmera e roda o *PointLandmarkVisualizer*, alinhando o esqueleto representado à rotação da câmera.
* **Update**: atualiza a imagem da aplicação com a textura lida pela câmera.

### Pose
Pose é a classe base de cada pose, contendo os seguintes métodos necessários para a criação e atualização dos pontos desta mesma:
	
* **Init**: Segundo as informações recebidas, instancia um novo objeto para cada ponto a criar, envia-lhes os seus pontos par e cria ligações entre eles. 
* **UpdatePoints**: este método trata de atualizar os pontos e os linerenderers, criados em Init, usando o método *UpdateValues* e *UpdateLineRenderers* da classe *Landmark*, com as informações que o *BlazePoseDetecter* disponibiliza, através do método *GetWorldPoseLandmark*.

### WebsocketClient
WebsocketClient é a classe responsável por estabelecer a conexão cliente-servidor, tratar da informação e fazer o envio dessa mesma. Esta classe define os seguintes métodos essenciais para o seu funcionamento:

* **Update**: Chama o método *ConvertAndSendMessage*. Só o consegue fazer depois do método *StartServerLog* ser chamado.
* **ConvertAndSendMessage**: Chamado pelo *Update*, quando conectado ao servidor, envia primeiramente a informação do frame e tempo atuais desde o início da conexão e depois a informação de cada ponto, através do método *SendMessage*.
* **SendMessage**: Converte a mensagem a enviar em Bytes e envia ao servidor via o método *Send* da classe *Websocket*.
* **InitWebsocketClient**: Usa o IP recebido e o port definido para estabelecer a conexão e adiciona o método *OnServerOpen* como observador do evento *OnOpen* da classe *Websocket*.
* **OnServerOpen**: Guarda o tempo de conexão ao servidor e começa o envio do log inicial, pelo método *InitialLog*.
* **InitialLog**: Envia as informações da quantidade de pontos e das conexões detectadas pelo programa ao servidor, pelo método *SendMessage*.
* **StartServerLog**: Permite o método Update de chamar o método *ConvertAndSendMessage*.

### PoseSaver
Responsável por guardar a pose atual. A funcionalidade planeada de guardar a pose num ficheiro nunca foi finalizada. Esta classe define os seguintes métodos essenciais para o seu funcionamento:
	
* **Start** : Inicializa esta classe e quaisquer referências necessárias ao seu funcionamento.
* **SaveCurrentPose**: inicializa uma pose nova, chamando o método *InitializePose`* da classe *PontLandmarkVisualizer*, usando os dados da pose atual, efetivamente criando uma cópia, e de seguida inicia o *PoseSimilarityComparer*, chamando o método *StartComparer* com a nova pose criada.

### PoseSimilarityComparer
Responsável por avaliar o quão similar é a pose atual e uma pose guardada. Esta classe define os seguintes métodos essenciais para o seu funcionamento:
* **Update**: Se houver uma pose guardada, calcula as direções das conexões da pose atual, pelo método *GetPoseLineDirections*, calcula a similaridade entre a pose guardada e a pose atual, pelo método *CalculatePoseSimilarity*, e finalmente atualiza o UIController com o valor dessa similaridade, via o método *UpdatePoseSimilarityScore*.
* **StartComparer**: Recebe e guarda a pose a guardar, e calcula as direções das suas ligações, usando o método *GetPoseLineDirections*.
* **GetPoseLineDirections**: Para cada ponto da pose guardada, encontra o vetor direção entre o mesmo e os pontos seguintes que formam as ligações.
* **DisplayIndividualSimilarityScores**: Atualiza cada segmento da pose guardada pela classe PoseSaver com as suas pontuações obtidas pelo método *CalculatePoseSimilarity*.
* **CalculatePoseSimilarity**: calcula e guarda a pontuação média da similaridade entre todas as ligações correspondentes das duas poses, cada uma obtida através do método *CosineSimilarity*. Guarda também uma lista com cada pontuação individual.
* **CosineSimilarity**: aplica o método matemático com o mesmo nome para comparar a direção entre dois vetores. 1 representa a mesma direção, -1 representa direções opostas e 0 representa direções oblíquas.

### UIController
Responsável por dar funcionalidade e controlar diversos elementos de UI. Esta classe define os seguintes métodos essenciais para o seu funcionamento:

* **Update**: Se a o temporizador para guardar a pose atual ou de ligação ao servidor estiver ativo, atualiza estes mesmos pelos métodos UpdatePoseCountdown e UpdateServerCountDown, respectivamente.
* **StartPoseSave**: inicia o temporizador para guardar a pose atual.
* **UpdatePoseCountdown**: atualiza o temporizador decrescentemente. Quando este chega a 0, desliga-o, e chama o método *SaveCurrentPose* da classe *PoseSaver*.
* **UpdatePoseSimilarityScore**: atualiza o elemento de UI que representa esta pontuação.
* **StartServerLogCountDown**: inicia o temporizador para enviar dados ao servidor.
* **UpdateServerCountdown**: atualiza o temporizador decrescentemente. Quando este chega a 0, desliga-o, e chama o método StartServerLog da classe *WebsocketClient*.
* **TryServerConnection**: Chama o método *InitWebsocketClient*. Se este devolver True, fecha o UI das opções e liga o UI da ligação ao servidor
