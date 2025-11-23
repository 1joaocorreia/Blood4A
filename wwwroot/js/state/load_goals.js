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

function parse_condition(condition, json_data) {

    let lcondition = condition.toLowerCase();
    // x > x
    // donations[2] > donations[5]
    // ...

    // replacing donations[x] for its respective value
    last_ocurrence = 0
    preventive_loop = 0
    while (true){
        preventive_loop++   
        if (preventive_loop >= 5) {
            break
        }
        // looking for the location of multiple "donation"
        last_ocurrence = lcondition.indexOf("donation", last_ocurrence)
        if (last_ocurrence == -1) {
            break
        }
        console.log("Last ocurrence: " + last_ocurrence)

        // extracting the index
        let start_bracket
        let end_bracket
        
        start_bracket = lcondition.indexOf('[', last_ocurrence)
        end_bracket = lcondition.indexOf(']', last_ocurrence)
        console.log("Start bracket: " + start_bracket)
        console.log("End bracket: " + end_bracket)

        if (start_bracket == -1 || end_bracket == -1) {
            break
        }

        const index = lcondition.slice(start_bracket + 1, end_bracket).trim()
        if (index < 1 || index > 12) {
            break
        }
        console.log("Index: " + index)
        // replacing the entire expression
        const data = json_data.doacoesPorMes[index-1].quantidadeDeDoacoes
        lcondition = lcondition.replace(`${lcondition.slice(last_ocurrence, start_bracket)}[${index}]`, data)
        
        last_ocurrence = last_ocurrence + 8
        continue;

    }

    // returning the value
    return lcondition

}

async function carregar_metas() {

    const current_url = window.location.href;
    const segments = current_url.split('/');
    const state = segments[segments.length - 1];

    const goals_endpoint = `${rootaddress}/api/info/state_goals/${state}`
    const goals_response = await fetch(goals_endpoint);
    if (! goals_response.ok)
    {
        $("div#metas").append(
            generate_bootstrap_error_card("Não foi possivel solicitar os dados de metas deste estado")
        );
        return;
    }

    const donations_endpoint = `${rootaddress}/api/info/state_donations/${state}`
    const donations_response = await fetch(donations_endpoint)
    if (! donations_response.ok)
    {
        $("div#metas").append(
            generate_bootstrap_error_card("Não foi possivel solicitar os dados informativos desta clinica")
        );
        return;
    }

    const goals_json = await goals_response.json();
    const donations_json = await donations_response.json();

    goals_json.forEach(cond => {
        
        console.log(cond)
        const parsed_condition = parse_condition(cond.condicao, donations_json)
        console.log("Condition parsed: " + parsed_condition)
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