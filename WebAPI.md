## API (Application Programming Interface)

Para consumir os recursos de um serviço RESTful usamos o protocolo HTTP e os verbos:
GET, POST, PUT, DELETE.

Recurso   | Descrição | Descrição
--------- | ------    | ------
GET       | Leitura   | 200 OK, 404 NOT FOUND
POST      | Criação   | 201 CREATERD, 204 NOT CONTENT
PUT       | Alteração | 200 OK, 204 NOT CONTENT, 404 NOT FOUND
DELETE    | Exclusão  | 200 OK, 204 NO CONTENT, 404 NOT FOUND

Código de Status HTTP

Código    | Descrição 
--------- | ------    
1XX       | Informativo     
2XX       | Sucesso      
3XX       | Redirecionamento     
4XX       | Erro do cliente (regra de negócio)   
5XX       | Erro do servidor 