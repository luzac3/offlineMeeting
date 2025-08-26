import { FetchApi } from "@root/share/FetchApi"

export class SendAnswer {
    private url: string;
    private method: string;
    private headers: { [key: string]: string }; 
    private responseKind: string;

    constructor() {
        this.url = '/Quiz/SendAnswer';
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
        const quizId = (document.getElementById('quiz_id') as HTMLInputElement).value;
        sendElement.onclick = () => {
            const dataUrl = canvasElement.toDataURL("image/png");
            const answerEntity: { [key: string]: string } = {
                quizId: quizId,
                image: dataUrl
            };
            this.send(answerEntity).then((data: string) => {
                const result = JSON.parse(data) as { [key: string]: string };
                console.log(data);
                window.alert(result.message);
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