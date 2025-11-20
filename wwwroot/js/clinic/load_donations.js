const host = "localhost"
const port = 7298
const rootaddress = `https://${host}:${port}`

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

    const canvas_chart = document.getElementById('donations_chart');

    if (canvas_chart == null) {
        $("div.donation_statistics").append(generate_bootstrap_error_card(
            "Um erro interno ocorreu. Não foi possivel carregar o gráfico de doações mensais"
        ));
        return;
    }

    const current_url = window.location.href;
    const segments = current_url.split('/');
    const clinic_id = segments[segments.length - 1];

    const endpoint = `${rootaddress}/api/info/clinic_donations/${clinic_id}`

    const response = await fetch(endpoint);
    if (! response.ok) {
        $("canvas#donations_chart").remove();
        $("div.donation_statistics").append(generate_bootstrap_error_card(
            "Ocorreu um erro ao requisitar os dados de doação desta clínica! Tente novamente mais tarde"
        ));
        return;
    }
    const json = await response.json();

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