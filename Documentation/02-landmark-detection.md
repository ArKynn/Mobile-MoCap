# Landmark Detection

Este projeto tem como base encontrar uma solução para “Motion capture em um dispositivo móvel com o uso de uma câmera”. 

Após pesquisas, foram encontradas as tecnologias Landmark Detection e Pose Estimation, que podem ser adaptadas para este efeito.		

Das duas, Landmark Detection apresentava melhor suporte para o pretendido, não só com outros projetos externos sobre o tema, mas também através da framework pré-existente [MediaPipe Pose](https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker), criada pela Google.

Um dos projetos externos encontrados e explorados foi o [BlazePoseBarracuda](https://github.com/creativeIKEP/BlazePoseBarracuda), que implementa a framework [MediaPipe Pose](https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker) em Unity Engine. Este projeto também apresenta-se público e com cenas Unity já montadas e prontas a usar, o que facilita a compreensão desta tecnologia.
Como o projeto encontrado disponibiliza o código correspondente ao detector responsável por realizar a Landmark Detection e mostrou bons resultados, foi escolhido para ser trabalhado em cima.

[Back](../README.md)