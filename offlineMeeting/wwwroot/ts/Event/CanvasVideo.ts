import { GetNewResolutions } from "@root/share/GetNewResolutions"
import { StartVideoMedia } from "./StartVideoMedia"

export class CanvasVideo {
    private resolution: { [key: string]: number };
    point: number = 25000;
    myMovingPoint: number = 0;
    targetPoint: number = 0;
    calcPoint: number = 0;
    leftCalcPoint: number = 0;
    isCaliculate: boolean = false;
    resultCd: string = "0";
    private image: HTMLImageElement;
    private title: string;
    private name: string;
    private videoNumber: number;
    private canvasResolutions: { [key: string]: { [key: string]: number } } = {};

    private canvas: HTMLCanvasElement | null = null;
    private ctx: CanvasRenderingContext2D | null = null;
    private videoElement: HTMLVideoElement | null = null;
    private isPointView: boolean = false;

    private resultDic: { [key: string]: string } = {
        "0": "　",
        "1": "ツモ",
        "2": "ロン",
        "3": "放銃",
        "30": "流局",
        "31": "聴牌",
        "32": "ノーテン",
        "33": "四風連打" ,
        "34": "九種九牌" ,
        "35": "四家立直" ,
        "36": "四開槓" ,
        "21": "ダブロン" ,
        "22": "トリロン",
    };

    constructor(title: string, name: string, videoNumber: number, point: number = 25000) {
    // 解像度を設定
        //this.resolution = { w: 1280, h: 960 };
        this.resolution = { w: 800, h: 600 };

        // canvasのwrapperサイズを取得
        const canvasWrapperElement = document.getElementById("canvas" + videoNumber + "Wrapper") as HTMLElement;
        this.canvasResolutions = GetNewResolutions.Get(canvasWrapperElement, this.resolution, "VGA");

        this.image = new Image();
        this.title = title;
        this.name = name;
        this.videoNumber = videoNumber;
        this.point = point;
    }

    activateCanvas = async () => {
        // 選択されているカメラの取得
        const myCameraSelectorElement = document.getElementById(
            "cameraSelector" + this.videoNumber
        ) as HTMLInputElement;
        const startVideoMedia = new StartVideoMedia();
        this.videoElement = await startVideoMedia.setVideo(
            myCameraSelectorElement.value,
            "video_" + this.videoNumber,
            this.resolution.w,
            this.resolution.h
        );

        return await this.makeCanvasVideo();
    }

    private makeCanvasVideo = async () => {
        // canvas要素を取得
        this.canvas = document.getElementById("canvasVideo_" + this.videoNumber) as HTMLCanvasElement;
        this.canvas.width = this.canvasResolutions["canvas"].w;
        this.canvas.height = this.canvasResolutions["canvas"].h;

        // コンテキストを取得する
        this.ctx = this.canvas.getContext('2d')!;

        // imageを読み込む
        await this.loadImage();

        // fixme 画像はそれごとに変えます コレの変更機能は後で
        this.image.src = "/image/pict1.png";

        this.isPointView = false;

        return this.drawCanvas();
    }

    // 一度ActivateしたあとはCanvasの描画だけ行うのでこのメソッドはPublic
    drawCanvas = () => {
        if (this.canvas && this.ctx) {
            if (this.videoElement === null) {
                throw new Error("video_" + this.videoNumber + "is not exist");
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

            // isCaliculateがONになったら点数移動の計算を行う
            if (this.isCaliculate) {
                this.pointMovingProcess();
            }

            // 文字枠表示
            this.ctx.globalAlpha = 0.5;
            this.ctx.fillRect(
                this.canvasResolutions["characterFrame"].x,
                this.canvasResolutions["characterFrame"].y,
                this.canvasResolutions["characterFrame"].w,
                this.canvasResolutions["characterFrame"].h
            );
            // 画像表示
            this.ctx.globalAlpha = 1;
            this.ctx.drawImage(
                this.image,
                this.canvasResolutions["face"].x,
                this.canvasResolutions["face"].y,
                this.canvasResolutions["face"].w,
                this.canvasResolutions["face"].h
            );
            this.ctx.textAlign = "end";
            this.ctx.fillStyle = "white";
            if (this.isPointView) {
                this.ctx.font = this.canvasResolutions["kind"].font.toString(),  + 'px monospace';
                this.ctx.fillText(
                    this.resultDic[this.resultCd],
                    this.canvasResolutions["kind"].x,
                    this.canvasResolutions["kind"].y
                );
                this.ctx.fillText(
                    this.myMovingPoint.toString(),
                    this.canvasResolutions["movingPoint"].x,
                    this.canvasResolutions["movingPoint"].y
                );
                this.ctx.font = this.canvasResolutions["allow"].font.toString() + 'px monospace';
                this.ctx.fillText(
                    "↓",
                    this.canvasResolutions["allow"].x,
                    this.canvasResolutions["allow"].y
                );
                this.ctx.textAlign = "end";
            } else {
                this.ctx.font = this.canvasResolutions["title"].font.toString() + 'px monospace';
                this.ctx.fillText(
                    this.title,
                    this.canvasResolutions["title"].x,
                    this.canvasResolutions["title"].y,
                );
                this.ctx.font = this.canvasResolutions["name"].font.toString() + 'px monospace';
                this.ctx.fillText(
                    this.name,
                    this.canvasResolutions["name"].x,
                    this.canvasResolutions["name"].y
                );
            }

            this.ctx.font = this.canvasResolutions["point"].font.toString()  + 'px monospace';
            this.ctx!.fillText(
                this.point.toLocaleString(),
                this.canvasResolutions["point"].x,
                this.canvasResolutions["point"].y
            );
        }
    }

    private loadImage = async () => {
        return this.image.addEventListener("load", () => { });
    }

    private pointMovingProcess = () => {
        this.isPointView = true;

        if (this.calcPoint !== 0) {
            this.leftCalcPoint -= this.calcPoint;
            this.point += this.calcPoint;
        }

        if (this.myMovingPoint < 0) {
            // 現ポイントがtargetPointより小さくなるまでfalse
            this.isCaliculate = this.point > this.targetPoint;
        } else {
            // 現ポイントがtargetPointより大きくなるまでfalse
            this.isCaliculate = this.point < this.targetPoint;
        }

        if (!this.isCaliculate) {
            this.leftCalcPoint = 0;
            this.calcPoint = 0;
            this.point = this.targetPoint;
            this.targetPoint = 0;
            window.setTimeout(() => {
                this.myMovingPoint = 0;
                this.isPointView = false;
            }, 1000 * 2);
        }
    }
}