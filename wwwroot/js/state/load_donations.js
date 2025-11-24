function generate_bootstrap_error_card(message) {
    return `
        <div class="card text-center text-white bg-danger" style="width: 100%;">
            <div class="card-body">
                ${message}
            </div>
        </div>
    `
}

async function carregar_grafico() {

    const canvas_chart = document.getElementById("donations_chart");

    if (canvas_chart == null) {
        $("div.donation_statistics").append(generate_bootstrap_error_card(
            "Um erro interno ocorreu. Não foi possivel carregar o gráfico de doações mensais"
        ));
        return;
    }

    const current_url = window.location.href;
    const segments = current_url.split("/");
    const estado = segments[segments.length - 1];

    const json = await request_state_donations(estado)

    let chart_data = []
    json.doacoesPorMes.forEach(doacao_mes => {
        chart_data.push(doacao_mes.quantidadeDeDoacoes)
    });

    new Chart(canvas_chart, {
        type: 'bar',
        data: {
            labels: ['Janeiro', 'Fevereiro', 'Marco', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
            datasets: [{
                label: 'Numero de Doações',
                data: chart_data,
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });

    return;
}

carregar_grafico();