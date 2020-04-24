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