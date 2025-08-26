import { FetchApi } from "@root/share/FetchApi"

export class SendCanvasImage {
    private url: string;
    private method: string;
    private headers: { [key: string]: string }; 
    private responseKind: string;

    constructor() {
        this.url = '/Quiz/SendCanvasImage';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }

    SendCanvas = () => {
        const sendElement = document.getElementById('send') as HTMLButtonElement;
        const canvasElement = document.getElementById('textCanvas') as HTMLCanvasElement;
        const quizId = (document.getElementById('quizId') as HTMLInputElement).value;
        sendElement.onclick = () => {
            const dataUrl = canvasElement.toDataURL("image/png");
            const answerEntity: { [key: string]: string } = {
                quizId: quizId,
                image: dataUrl
            };
            this.send(answerEntity).then((data: string) => {
                console.log(data);
                alert('送信完了');
            });
        };
    }

    private send = async (answerEntity: { [key: string]: string }) => {
        const fetchApi = new FetchApi();
        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            answerEntity,
            this.responseKind
        ).then(async (data: string) => {
            return data;
        }).catch(e => {
            throw e;
        });
    }
}