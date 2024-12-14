var toastTimeOut;

$(document).ready(function() {
    const connection = new window.signalR.HubConnectionBuilder().withUrl("/hub").build();

    connection.start().then(() => { console.log("Bağlantı kuruldu"); })
    connection.on("AlertCompleteFile",(downloadPath) => {
        clearTimeout(toastTimeOut);
        $(".toast-body").html(`
        <p>Excell oluşturma işlemi tamamlanmıştır. aşağıdaki link ile excell dosyasını indirebilirsiniz</p>
        <a href="${downloadPath}>İndir</a>"
        `);
        $("#liveToast").show();
    })
})