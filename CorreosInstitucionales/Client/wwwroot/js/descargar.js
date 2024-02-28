function Descargar(url)
{
    const enlace = document.createElement('a');

    enlace.href = url;
    enlace.download = "";

    enlace.click();
    enlace.remove();
}
