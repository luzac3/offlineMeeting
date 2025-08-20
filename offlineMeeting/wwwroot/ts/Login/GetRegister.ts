import { SignalR } from "@root/share/SignalR";

export class GetRegister {
    registerComplete = ((signalR: SignalR) => {
        signalR.get("RegisterSuccess", (url) => {
            const parsedUrl = <{ [key: string]: string }>url;
            location.href = parsedUrl.url;
        });
    });
}