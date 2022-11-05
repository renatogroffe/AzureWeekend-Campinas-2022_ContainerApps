# AzureWeekend-Campinas-2022_ContainerApps
Exemplos para uso com Azure Container Apps. Objetos para Deployment de um Worker Service (apuração de votos de tecnologias + Feature Flags) no Kubernetes utilizando KEDA, Helm, Apache Kafka (a partir de uma instância do Azure Event Hubs) e .NET 6.

**WorkerQuestao** -- Worker Service para consumo de **mensagens vinculadas a um tópico do Apache Kafka** (imagem **renatogroffe/workerquestao-kafka-appinsights-featureflags-dotnet6:latest**).

**SiteQuestao** -- Projeto que serviu de base para o **envio de mensagens a um tópico do Apache Kafka** (imagem **renatogroffe/sitequestao-kafka-appinsightsconnstring-dotnet6**).

No arquivo **keda-instalacao&sdot;sh** estão as instruções para instalação do KEDA **(Kubernetes Event-driven Autoscaling)** em um **cluster Kubernetes**.

Há aqui também um pipeline para testes de carga randômicos com a ferramenta **k6**.