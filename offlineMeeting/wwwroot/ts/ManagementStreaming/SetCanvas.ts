import { GetNewResolutions } from "@root/share/GetNewResolutions"

export class SetCanvas {
    private canvasElement = document.getElementById("streamingCanvasVideo") as HTMLCanvasElement;
    private resolution: { [key: string]: number };
    private ctx: CanvasRenderingContext2D | null = null;
    private videoElement: HTMLVideoElement;
    private canvasResolutions: { [key: string]: { [key: string]: number } } = {};
    private fps: number = 60;

    constructor() {
        this.resolution = { w: 800, h: 600 };
        this.videoElement = document.getElementById("video_0") as HTMLVideoElement;
    }

    setDrawCanvas = async () => {
        this.makeCanvasVideo();

        await window.setInterval(() => {
            this.drawCanvas();
        }, 1000 / this.fps);
    }

    setVideoElement = (_videoElement: HTMLVideoElement) => {
        this.videoElement = _videoElement;
    }

    private makeCanvasVideo = async () => {
        // canvasのwrapperサイズを取得
        const canvasWrapperElement = document.getElementById("canvasWrapper") as HTMLElement;
        this.canvasResolutions = GetNewResolutions.Get(canvasWrapperElement, this.resolution, "VGA");

        this.canvasElement.width = this.canvasResolutions["canvas"].w;
        this.canvasElement.height = this.canvasResolutions["canvas"].h;

        // コンテキストを取得する
        this.ctx = this.canvasElement.getContext('2d')!;
    }

    private drawCanvas = () => {
        if (this.canvasElement && this.ctx) {
            if (this.videoElement === null) {
                throw new Error("video is not exist");
            }

            this.ctx.globalAlpha = 1;
            // バックグラウンド表示
            this.ctx.fillStyle = "black";

            this.ctx.fillRect(
                0,
                0,
                this.canvasResolutions["canvas"].w,
                this.canvasResolutions["canvas"].h
            );

            // video表示
            this.ctx.drawImage(
                this.videoElement,
                this.canvasResolutions["video"].x,
                this.canvasResolutions["video"].y,
                this.canvasResolutions["video"].w,
                this.canvasResolutions["video"].h
            );
        }
    }
}