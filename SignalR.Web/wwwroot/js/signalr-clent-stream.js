$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub").configureLogging(signalR.LogLevel.Information).build();

    async function start() {
        try {
            await connection.start().then(() => {
                console.log("Hub ile bağlantı kuruldu");
                /*  $("#connectionId").html(`Connection Id: ${connection.connectionId}`);*/
            });
        } catch (e) {
            console.log("Hata ", e.message);
            setTimeout(() => start(), 5000);
        }
    }

    connection.onclose(async () => {
        await start();
    });

    start();

    const broadcastStreamDataToAllClient = "BroadcastStreamDataToAllClient";
    const receiveMessageAsStreamForAllClient = "ReceiveMessageAsStreamForAllClient";

    $("#btn-FromClient-ToHub").click(function () {
        const names = $("#txt-stream").val();
        const nameAsChunk = names.split(";");
        const subject = new signalR.Subject();
        connection.send(broadcastStreamDataToAllClient, subject).catch(err => console.error(err));
        nameAsChunk.forEach(name => {
            subject.next(name)
        });
        subject.complete();
    });

    connection.on(receiveMessageAsStreamForAllClient,
        (name) => {
            $("#streamBox").append(`<p>${name}</p>`);
        });

    const broadcastStreamProductToAllClient = "BroadcastStreamProductToAllClient";
    const receiveProductAsStreamForAllClient = "ReceiveProductAsStreamForAllClient";


    $("#btn-FromClient-ToHub2").click(function () {
        const productList = [
            { id: 1, name: "pen 1", price: 100 },
            { id: 2, name: "pen 2", price: 100 },
            { id: 3, name: "pen 3", price: 100 }
        ];
        const subject = new signalR.Subject();
        connection.send(broadcastStreamProductToAllClient, subject).catch(err => console.error(err));
        productList.forEach(product => {
            subject.next(product)
        });
        subject.complete();
    });

    connection.on(receiveProductAsStreamForAllClient,
        (product) => {
            $("#streamBox").append(`<p>${product.id}--${product.name}--${product.price}</p>`);
        });

    const broadcastFromHubToClient = "BroadcastFromHubToClient";

    $("#btn-FromGub-ToClient").click(function() {
        connection.stream(broadcastFromHubToClient, 5).subscribe({
            next: (message) => $("#streamBox").append(`<p>${message}</p>`)
        });
    });

})