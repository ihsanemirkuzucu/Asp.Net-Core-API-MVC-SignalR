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
    };

    connection.onclose(async () => {
        await start();
    });

    start();

    const broadcastMessageToAllClientHubMethodCall = "BroadcastMessageToAllClient";
    const receiveMessageForAllClientMethodCall = "ReceiveMessageForAllClient";

    $("#btn-send-message-all-client").click(function() {
        const message = "Hello word";
        connection.invoke(broadcastMessageToAllClientHubMethodCall, message).catch(err => console.error("hata", err));
    });

    connection.on(receiveMessageForAllClientMethodCall,
        (message) => {
            console.log("gelen mesaj:", message);
            console.log("Mesaj gönderildi");
        });

    const broadcastMessageToCallerClientHubMethodCall = "BroadcastMessageToCallerClient";
    const receiveMessageForCallerClientMethodCall = "ReceiveMessageForCallerClient";

    $("#btn-send-message-caller-client").click(function() {
        const message = "Hello word";
        connection.invoke(broadcastMessageToCallerClientHubMethodCall, message)
            .catch(err => console.error("hata", err));
    });

    connection.on(receiveMessageForCallerClientMethodCall,
        (message) => {
            console.log("(caller) gelen mesaj:", message);
            console.log("Mesaj gönderildi");
        });

    const broadcastMessageToOtherClientHubMethodCall = "BroadcastMessageToOtherClient";
    const receiveMessageForOtherClientMethodCall = "ReceiveMessageForOtherClient";

    $("#btn-send-message-other-client").click(function() {
        const message = "Hello word";
        connection.invoke(broadcastMessageToOtherClientHubMethodCall, message).catch(err => console.error("hata", err));
    });

    connection.on(receiveMessageForOtherClientMethodCall,
        (message) => {
            console.log("(other)gelen mesaj:", message);
            console.log("Mesaj gönderildi");
        });

    const broadcastMessageToIndividualClientHubMethodCall = "BroadcastMessageToIndividualClient";
    const receiveMessageForIndividualClientMethodCall = "ReceiveMessageForIndividualClient";

    $("#btn-send-message-individual-client").click(function() {
        const connectionId = $("#text-connectionId").val();
        const message = "Hello word";
        connection.invoke(broadcastMessageToIndividualClientHubMethodCall, connectionId, message)
            .catch(err => console.error("hata", err));
    });

    connection.on(receiveMessageForIndividualClientMethodCall,
        (product) => {
            console.log("(product) gelen mesaj:", product);
            console.log("Mesaj gönderildi");
        });

    const receiveConnectedClientCountAllClientMethodCall = "ReceiveConnectedClientCountAllClient";

    const span_client_count = $("#span-connected-client-count");
    connection.on(receiveConnectedClientCountAllClientMethodCall,
        (count) => {
            span_client_count.text(count);
            console.log("Connected Client Count:", count);
        });


    const broadcastTypedMessageToAllClientHubMethodCall = "BroadcastTypedMessageToAllClient";
    const receiveTypedMessageForAllClientMethodCall = "ReceiveTypedMessageForAllClient";

    $("#btn-send-typed-message-all-client").click(function() {
        const product = { id: 1, name: "Pen 1", price: 200 };
        connection.invoke(broadcastTypedMessageToAllClientHubMethodCall, product)
            .catch(err => console.error("hata", err));
        console.log("Ürün gönderildi");
    });

    connection.on(receiveTypedMessageForAllClientMethodCall,
        (message) => {
            console.log("gelen mesaj:", message);
            console.log("Mesaj gönderildi");
        });

    const groupA = "GroupA";
    const groupB = "GroupB";
    let currentGroupList = [];

    function refreshGroupList() {
        $("#groupList").empty();
        currentGroupList.forEach(x => {
            $("#groupList").append(`<p>${x}</p>`);
        });
    };

    $("#btn-groupA-add").click(function() {
        if (currentGroupList.includes(groupA)) return;

        connection.invoke("AddGroup", groupA).then(() => {
            currentGroupList.push(groupA);
            refreshGroupList();
        });
    });

    $("#btn-groupA-remove").click(function() {
        if (!currentGroupList.includes(groupA)) return;

        connection.invoke("RemoveGroup", groupA).then(() => {
            currentGroupList = currentGroupList.filter(x => x !== groupA);
            refreshGroupList();
        });
    });

    $("#btn-groupB-add").click(function() {
        if (currentGroupList.includes(groupB)) return;

        connection.invoke("AddGroup", groupB).then(() => {
            currentGroupList.push(groupB);
            refreshGroupList();
        });
    });

    $("#btn-groupB-remove").click(function() {
        if (!currentGroupList.includes(groupB)) return;

        connection.invoke("RemoveGroup", groupB).then(() => {
            currentGroupList = currentGroupList.filter(x => x !== groupB);
            refreshGroupList();
        });
    });

    $("#btn-groupA-send-message").click(function() {
        const message = "Group A Mesaj";
        connection.invoke("BroadcastMessageToGroupClient", groupA, message).catch(err => console.error("hata", err));
    });

    $("#btn-groupB-send-message").click(function() {
        const message = "Group B Mesaj";
        connection.invoke("BroadcastMessageToGroupClient", groupB, message).catch(err => console.error("hata", err));
    });

    connection.on("ReceiveMessageForGroupClient",
        (message) => {
            console.log("(group) gelen mesaj:", message);
            console.log("Mesaj gönderildi");
        });


})