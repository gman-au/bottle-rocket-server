function downloadBase64File(filename, contentType, base64Data) {
    const linkSource = `data:${contentType};base64,${base64Data}`;

    const downloadLink = document.createElement("a");
    downloadLink.href = linkSource;
    downloadLink.download = filename;

    downloadLink.click();
}
