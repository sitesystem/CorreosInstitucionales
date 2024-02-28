function Descargar(item)
{
    console.log(item);

    const enlace = document.createElement('a');

    enlace.href = item.url;

    if (item.name != "#")
    {
        enlace.download = item.name;
    }
    
    enlace.click();
    enlace.remove();
}
