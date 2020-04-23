## Programa desarrollado en Unity que accede a la galería de un teléfono Android para mostrar imágenes y videos


### Pasos previos 

*Instalar la versión 2019.3.7f1 de Unity*
*Si se desea hacer debug en el teléfono Android descargar de Play Store Unity Remote 5*

### Creación del programa

1. Iniciar Unity Hub y crear un nuevo programa 3D llamado GalleryAccess.
2. Ir a File, Build Settings ...
3. Seleccionar Android, dar clic en Switch Platform
4. En la parte inferior izquierda dar clic en Player Settings ...
5. Ir a Write Permission y cambiar a: External (SDCard)
6. En la pestaña Game cambiar la resolución de pestaña a 16:9 Portrait (9:16)
7. Dar clic derecho en la escena, UI, Button
7.1. Darle nombre: Select_image, posicionarla: Pos Y: 400, Width: 200, Height: 50, como texto colocar: Seleccionar imagen
8. Dar clic derecho en la escena, UI, Button
8.1. Darle nombre: Select_video, posicionarla: Pos Y: 350, Width: 200, Height: 50, como texto colocar: Seleccionar video
9. Importar plugin desde Asset Store: Native Gallery for Android & iOS
10. En la parte inferior, en Assets dar clic derecho y crear la carpeta Scripts
11. En esta carpeta crear un script de C# con el nombre de GalleryAccess
12. Copiar el siguiente código en el script

```c#
using UnityEngine;

public class GalleryAccess : MonoBehaviour
{
    public void ChooseVideo()
    {
        // Función privada para mostrar un
        // video en la aplicación
        PickVideo();
    }

    public void ChoosePhoto()
    {
        // Función privada para mostrar una imagen
        //  de 1024x1024 en la aplicación
        PickImage(1024);
    }

    private void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Ruta de la imagen: " + path);

            if (path != null)
            {
                // Se crea una textura de la imagen seleccionada
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);

                if (texture == null)
                {
                    Debug.Log("No se pudo cargar una textura de la ruta: " + path);
                    return;
                }

                // Se asigna la textura a un "quad" o plano y se destruye después
                // de 5 segundos para cargar otra imagen
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                quad.transform.forward = Camera.main.transform.forward;
                quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                Material material = quad.GetComponent<Renderer>().material;

                if (!material.shader.isSupported) 
                    material.shader = Shader.Find("Legacy Shaders/Diffuse");

                material.mainTexture = texture;

                Destroy(quad, 5f);
                Destroy(texture, 5f);
            }
        }, "Selecciona una imagen PNG", "image/png");

        Debug.Log("Resultado de los permisos: " + permission);
    }

    private void PickVideo()
    {
        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
        {
            Debug.Log("Ruta del video: " + path);

            if (path != null)
            {
                // Muestra el video seleccionado
                Handheld.PlayFullScreenMovie("file://" + path);
            }
        }, "Selecciona un video");

        Debug.Log("Resultado de los permisos: " + permission);
    }
}
```

13. Una vez copiado el código añadir el script a ambos botones.
14. Ir a File, Build Settings ...
15. Dar clic  en "Add Open Scenes" y Build
16. Probar
