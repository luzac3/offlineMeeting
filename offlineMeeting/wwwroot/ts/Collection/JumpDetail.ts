export class JumpDetail {
    Jump = () => {
        const elements = Array.from(document.getElementsByClassName('alcohol_order'));

        elements.forEach((element) => {
            if (element.classList.contains("detail_link")) {
                const alcoholInfo = element.querySelector('.alchol_info') as HTMLElement;
                alcoholInfo.addEventListener("click", () => {
                    const alcoholId = alcoholInfo.dataset.result_id;

                    location.href = `/Collection/AlcoholDetail/${alcoholId}`;
                });
            }
        });
    }
}