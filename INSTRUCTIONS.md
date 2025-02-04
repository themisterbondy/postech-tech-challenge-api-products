# Instruções para Deploy do MyFood WebAPI Products (Docker Local)

## Construção e Publicação da Imagem Docker
Execute os seguintes comandos para construir e publicar a imagem Docker da aplicação:

```sh
docker build -t themisterbondy/myfood-products-webapi -f src/Postech.Fiap.Products.WebApi/Dockerfile .

docker push themisterbondy/myfood-products-webapi
```

## Criação do Namespace
Crie o namespace para o MyFood WebAPI:

```sh
kubectl create namespace myfood-namespace
```

## ConfigMap para Banco de Dados do MyFood WebAPI
Crie um ConfigMap para armazenar as credenciais de conexão com o banco de dados:

```sh
kubectl create configmap myfood-db-products-config --namespace=myfood-namespace --from-literal=MongoDb__ConnectionString="mongodb://host.docker.internal:27017"
```

## Instalação do MyFood WebAPI com Helm
Para instalar a aplicação no Kubernetes usando Helm, execute:

```sh
helm install myfood-products-webapi .\charts\webapi\ --namespace myfood-namespace
```

## Atualização do MyFood WebAPI com Helm
Caso precise atualizar a aplicação, utilize o comando abaixo:

```sh
helm upgrade myfood-products-webapi .\charts\webapi\ --namespace myfood-namespace
```

## Redirecionamento de Porta no MyFood WebAPI
Utilize o seguinte comando para redirecionar a porta e acessar o serviço localmente:

```sh
kubectl port-forward svc/myfood-products-webapi 60076:80 -n myfood-namespace
```

## Monitoramento e Depuração

### Obter a Lista de Pods
Monitore os Pods do namespace com o comando:

```sh
kubectl get pods --namespace myfood-namespace --watch
```

### Acessar um Pod Interativamente
Entre em um Pod da aplicação de forma interativa usando o comando:

```sh
kubectl exec -it myfood-products-webapi-75ccdb8997-dg624 -- /bin/sh
```

### Descrever um Pod
Para obter detalhes de um Pod específico no Kubernetes, execute:

```sh
kubectl describe pod myfood-products-webapi-64d46cb67-nkbzl --namespace myfood-namespace
```

### Verificar Logs dos Pods
Confira os logs dos Pods com os comandos abaixo:

```sh
kubectl logs myfood-products-webapi-64d46cb67-nkbzl --namespace myfood-namespace
```

## Remover Recursos do MyFood WebAPI

### Remover o Deployment
Para remover apenas o deployment do MyFood WebAPI, utilize:

```sh
kubectl delete deployment myfood-products-webapi --namespace myfood-namespace
```

### Remover o Namespace
Caso precise excluir o namespace e todos os seus recursos, use:

```sh
kubectl delete namespace myfood-namespace
```