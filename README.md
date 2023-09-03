# TFG_retargeting
Proyecto final de grado para el grado de Ingeniería Informática en la Universidad Pública de Navarra (UPNa).
## Título
Desarrollo de herramienta software para la edición de animaciones para retargeting de personajes.
## Memoria
[PDF Memoria](Recursos/TFG_adrian_guiral_mallart.pdf)
### Introducción
El ‘animation retargeting’ o reorientación de animaciones es una función que permite la reutilización de animaciones entre personajes que utilizan el mismo esqueleto, pero no comparten las mismas dimensiones [1]. Esta técnica es muy útil debido a que reduce la carga de trabajo del animador enormemente y solventa el problema de animar la misma animación para cada personaje nuevo. Se trata de una técnica muy común y extendida principalmente dentro de la industria del videojuego, no obstante, también está presente en la industria del cine para ajustar animaciones a avatares 3D utilizados en la producción tanto de películas como series. Para facilitar este trabajo, se va a utilizar el motor de desarrollo Unity 3D, uno de los motores más conocidos a nivel mundial y con mayor disponibilidad, teniendo a su disposición una amplia lista de herramientas y versiones. 
### Objetivos
Los objetivos de este proyecto son muy claros, en primer lugar, se busca ajustar animaciones de un modelo 3D a modelos 3D con misma estructura ósea (‘rigging’) pero dimensiones irregulares de manera que la animación reorientada (‘retargeted’) opere de forma natural sobre los nuevos modelos. Esto se consigue a través de una edición mínima de la animación para solucionar errores causados por la naturaleza de las dimensiones del modelo. Se trata de una edición mínima para no provocar cambios en la finalidad de la animación. En segundo lugar, se persigue desarrollar una herramienta de fácil uso que permita un acercamiento accesible al mundo de la animación en el ámbito principalmente de la creación y desarrollo de videojuegos.
### Resumen
Se busca desarrollar una herramienta en Unity 3D con C# para la edición y modificación de animaciones sobre modelos 3D. Haciendo uso de la herramienta el usuario podrá observar la animación y modificarla a través de controles tales como controles deslizantes o indicadores 3D sobre huesos del modelo. La herramienta permitirá la creación de estas nuevas animaciones y su almacenamiento con el fin de realizar un 'retargeting', es decir, ajustar la animación original a distintos modelos 3D.
### Autor
Adrián Guiral Mallart
