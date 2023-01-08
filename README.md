# GolfWorld


### 1. Instalar Visual Studio 2019:
  - [ ] Instalar Visual Studio (Community) 2019 con los siguientes paquetes:
    - [ ] Cargas de trabajo -> Desarrollo de juego con Unity
    - [ ] Componentes individuales -> Extensión de GitHub para VS y GIT para Windows


### 2. Descargar Unity y enlazar con Visual Studio:
  - [ ] Descargar Unity 2021.3.16f1.
  - [ ] En Unity Hub -> Install -> Engranaje y Add modules -> Instalar Microsoft Visual Studio 2019


### 3. Descargar proyecto de GitHub:
  - [ ] Descargar proyecto del GitHub en formato ZIP y descomprimir
  - [ ] En Unity Hub -> Open -> seleccionar la carpeta descomprimida (GolfWorld-master) -> eleccionar el editor 2021.3.16f1 -> opción de no entrar en Safe Mode (Ahora arreglamos los errores) [Continue -> Ignore]


### 4. Instalar paquetes de Unity:
  - [ ] Dentro del proyecto, click derecho en Packages (pestaña Project) -> View in Package Manager
  - [ ] Instalar paquete Cinemachine: darle al + -> Add package from git URL... -> ```com.unity.cinemachine``` (Install)
  - [ ] Instalar paquete ProGrids: darle al + -> Add package from git URL... -> ```com.unity.progrids``` (Install)


### 5. Abrir projecto en Unity y Visual Studio:
  - [ ] En Unity, Edit -> Preferences... -> External Tools -> External Script Editor -> Visual Studio 2019
  - [ ] Asserts -> Open C# Project (Si miramos el Explorador de soluciones vemos el directorio Assets)


### 6. Enlazar el proyecto con el repositorio GitHub:
  - [ ] Ir al panel Cambios de GIT -> Crear repositorio de GIT... -> seleccionar Repositorio remoto existente en Otros -> Remote URL el [URL repo](https://github.com/samuel0122/GolfWorld.git) -> Crear y enviar cambios [salta error, pero se enlaza al repo]
  - [ ] Le damos a los 3 puntitos al lado del master, en el panel Cambios de GIT -> Abrir en símbolo del sistema, introducimos los comandos:
```   
        git pull origin master --allow-unrelated-histories
        git reset --hard origin/master
```


### 7. Ya está:
  - [ ] Ya tenemos el proyecto local enlazado al remoto. Podéis probarlo añadiendo un comentario, haciendo commit, pull y push al remoto (**__SIEMPRE__ hacer pull antes de un push y resolver conflictos si se dan**).
  - [ ] Para abrir el proyecto en VS tenéis que volver a abrirlo desde Assets -> Open C# Project en Unity. Si todo fue bien, VS os ayudará escribiendo código enseñando sugerencias.
  - [ ] Unity va por escenas. Para abrir una escena y poder editarla abrir la carpeta Scenes y doble click sobre una escena.
