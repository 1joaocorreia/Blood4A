function generate_bootstrap_error_card(message) {
    return `
        <div class="card text-center text-white bg-danger" style="width: 100%;">
            <div class="card-body">
                ${message}
            </div>
        </div>
    `
}

function generate_meta(description, accomplished)
{
    return `
        <div class="list-group-item">
            <h2>${description}</h2>
            <p>Accomplished? ${(accomplished == true) ? "YES" : "NO"}</p>
        </div>
    `
}

async function carregar_metas() {

    const current_url = window.location.href;
    const segments = current_url.split('/');
    const clinic_id = segments[segments.length - 1];

    const goals_json = await request_clinic_goals(clinic_id)
    const donations_json = await request_clinic_donations(clinic_id)

    goals_json.forEach(cond => {
        
        const parsed_condition = parse_condition(cond.condicao, donations_json)
        try {
            let veridict = eval(parsed_condition)
            $("div#metas").append(
                generate_meta(cond.condicao, veridict)
            )
        } catch {
            $("div#metas").append(
                generate_bootstrap_error_card("Não foi possivel avaliar a condição estabelecida")
            )
        }

    });


    return;

}

carregar_metas();