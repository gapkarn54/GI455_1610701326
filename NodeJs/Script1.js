var websocket = require('ws');

var callbackInitServer = ()=>
{
    console.log("server is running.");
}

var wss = new websocket.Server({port:15726} , callbackInitServer)
var wslist = [];
wss.on("connection",(ws)=>
{
    wslist.push(ws);
            for(var i = 0; i<wslist.length; i++)
        {
            if(wslist[i] == ws)
            {
                console.log("client " + i + " connected");
                break;
            }
        }
    ws.on("message", (data)=>
    {
        for(var i = 0; i < wslist.length; i++)
        {
            if(wslist[i] != ws)
            {
                wslist[i].send(data + "                                                                       ");
                continue;
            }
            if(wslist[i] == ws)
            {
                wslist[i].send("Client " + i + " : " + data);
                console.log("sent from client " + i + " : " + data);
                continue;
            }
        }
    });

    ws.on("close", ()=>
    {
        /*for(var i = 0; i<wslist.length; i++)
        {
            if(wslist[i] == ws)
            {
                wslist.splice(i,1);
                break;
            }
        }*/
        for(var i = 0; i<wslist.length; i++)
        {
            if(wslist[i] == ws)
            {
                console.log("client " + i + " disconnected");
                wslist = RemoveArray(wslist , ws);
                break;
            }
        }
    });

});


function RemoveArray(arr, value)
{
    return arr.filter((element)=>
    {
        return element != value;
    });
}
/*function Boardcast(data)
{
    for(var i = 0;i<wslist.length;i++)
    {
        wslist[i].send(data);
    }
}*/