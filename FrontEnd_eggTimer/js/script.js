

//// BUSCAR TIMERTASK POR ID ////////////////////////////////////////////////

async function buscarTimerTaskPorId() {
    const taskId = document.getElementById("taskId").value;
    if (!taskId) return alert("Digite um ID!");

    try {
        const res = await fetch(`https://localhost:7296/TimerTask/id/${taskId}`);
        if (!res.ok) throw new Error("Não encontrado!");
        
        const data = await res.json();
        document.getElementById("resultado").innerHTML = `
            <p><strong>Nome:</strong> ${data.nome}</p>
            <p><strong>Tempo:</strong> ${data.horarioCronometrado}</p>
            <p><strong>Status:</strong> ${data.status}</p>
        `;
    } catch (error) {
        document.getElementById("resultado").innerHTML = `<p style="color: red;">Erro: ${error.message}</p>`;
    }
}

//// BUSCAR TIMERTASK POR NOME ////////////////////////////////////////////////

async function buscarTimerTaskPorNome() {
    const taskNome = document.getElementById("taskNome").value;
    if (!taskNome) return alert("Digite um NOME!");

    try {
        const res = await fetch(`https://localhost:7296/TimerTask/nome/${taskNome}`);
        if (!res.ok) throw new Error("Não encontrado!");
        
        const data = await res.json();
        document.getElementById("resultado2").innerHTML = `
            <p><strong>Nome:</strong> ${data.nome}</p>
            <p><strong>Tempo:</strong> ${data.horarioCronometrado}</p>
            <p><strong>Status:</strong> ${data.status}</p>
        `;
    } catch (error) {
        document.getElementById("resultado").innerHTML = `<p style="color: red;">Erro: ${error.message}</p>`;
    }
}


//// CARREGAR LISTA DE TAREFAS////////////////////////////////////////////////


async function carregarTarefas() {
    try {
        const res = await fetch("https://localhost:7296/TimerTask");
        if (!res.ok) throw new Error("Erro ao carregar tarefas!");
        
        document.getElementById("timeInput").readOnly = true;
        document.getElementById("nomeInput").readOnly = true;

        const tarefas = await res.json();
        const lista = document.getElementById("listaTarefas");
        lista.innerHTML = ""; 

        tarefas.forEach((tarefa, index) => {
            const option = document.createElement("option");
            option.value = tarefa.nome;
            option.textContent = `${tarefa.nome} - ${tarefa.horarioCronometrado} - ${tarefa.status}`
            lista.appendChild(option);
        });
    } catch (error) {
        alert(error.message);
    }
}

let timer;

function calcularTempoRestante(horarioFim) {
    const now = new Date();
    
    const [horas, minutos, segundos] = horarioFim.split(":").map(Number);
    const tempoFim = new Date(now);
    tempoFim.setHours(horas, minutos, segundos, 0); 

    const diff = tempoFim - now;
    
    if (diff <= 0) {
        return 0;
    }

    return Math.floor(diff / 1000);
}


function iniciarContagem(totalSegundos) {
    clearInterval(timer);

    timer = setInterval(() => {
        if (totalSegundos <= 0) {
            clearInterval(timer);
            carregarTarefas();
            document.getElementById("timeInput").value = "00:00:00";
            return;
        }

        totalSegundos--;

        let h = String(Math.floor(totalSegundos / 3600)).padStart(2, '0');
        let m = String(Math.floor((totalSegundos % 3600) / 60)).padStart(2, '0');
        let s = String(totalSegundos % 60).padStart(2, '0');

        document.getElementById("timeInput").value = `${h}:${m}:${s}`;
    }, 1000);
}



async function mostrarOpcoes() {
    const lista = document.getElementById("listaTarefas");
    //const opcoes = document.getElementById("acoesTarefa");
    //const mensagem = document.getElementById("mensagem");
    const timeInput = document.getElementById("timeInput");
    const nomeInput = document.getElementById("nomeInput");
    const imagemStatus = document.getElementById("imagemStatusEgg");
    document.getElementById("btnSalvar").disabled = true;
    try {
        const res = await fetch(`https://localhost:7296/TimerTask/nome/${lista.value}`);
        if (!res.ok) throw new Error("Não encontrado!");

        const data = await res.json();
        clearInterval(timer);

        if(data.status == "Em Andamento"){
            const tempoRestante = calcularTempoRestante(data.horarioFim);
            imagemStatus.src = "../images/egg_in_progress.gif";
            iniciarContagem(tempoRestante);
        }
        else{
            imagemStatus.src = "../images/egg_completed.gif";
            timeInput.value = "00:00:00";
        }
        //timeInput.value = data.horarioFim;
        nomeInput.value = `${data.nome}`;
        //mensagem.innerText = `${data.status}`;
        timeInput.readOnly = true; 
        nomeInput.readOnly = true;
        //opcoes.style.display = "block";
        
    } catch (error) {
        alert(error.message);
        //opcoes.style.display = "none";
    }
}





///////////////////////////////// editar tarefa//////////////////////


async function editarTarefa() {
    const lista = document.getElementById("listaTarefas");
    const btnSalvar = document.getElementById("btnSalvar");
    const tarefaNome = lista.value;

    try {
        const res = await fetch(`https://localhost:7296/TimerTask/nome/${tarefaNome}`);
        if (!res.ok) throw new Error("Tarefa não encontrada!");

        const tarefa = await res.json();
        const timeInput = document.getElementById("timeInput");
        const nomeInput = document.getElementById("nomeInput");
        timeInput.readOnly = false;
        nomeInput.readOnly = false;
        //btnSalvar.style.display = "block";
        btnSalvar.disabled = false;
        clearInterval(timer);
        nomeInput.value = tarefa.nome;
        timeInput.value = tarefa.horarioCronometrado;

        btnSalvar.onclick = async function () {
            const novoNome = nomeInput.value;
            const novoTempo = timeInput.value;

            const updatedTask = {
                nome: novoNome,
                horarioCronometrado: novoTempo
            };

            const updateRes = await fetch(`https://localhost:7296/TimerTask/${tarefa.id}`, {
                method: "PUT", 
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(updatedTask),
            });

            if (updateRes.ok) {
                timeInput.readOnly = true; 
                nomeInput.readOnly = true;
                alert("Tarefa atualizada com sucesso!");
                carregarTarefas();
            } else {
                alert("Erro ao atualizar a tarefa!");
            }
        };

        
    } catch (error) {
        alert(error.message);
    }
}




// async function salvarAlteracaoTarefa() {
//     const novoNome = nomeInput.value;
//     const novoTempo = timeInput.value;

//     const updatedTask = {
//         nome: novoNome,
//         horarioCronometrado: novoTempo
//     };

//     const updateRes = await fetch(`https://localhost:7296/TimerTask/${tarefa.id}`, {
//         method: "PUT", 
//         headers: { "Content-Type": "application/json" },
//         body: JSON.stringify(updatedTask),
//     });

//     if (updateRes.ok) {
//         btnSalvar.style.display = "none";
//         timeInput.readOnly = true; 
//         nomeInput.readOnly = true;
//         alert("Tarefa atualizada com sucesso!");
//         carregarTarefas();
//     } else {
//         alert("Erro ao atualizar a tarefa!");
//     }
// }


///////////////////////////////// EXCLUIR tarefa//////////////////////


async function excluirTarefa() {
    const lista = document.getElementById("listaTarefas");
    const timeInput = document.getElementById("timeInput");
    const nomeInput = document.getElementById("nomeInput");
    const tarefaNome = lista.value;

    try {
        const res = await fetch(`https://localhost:7296/TimerTask/nome/${tarefaNome}`);
        if (!res.ok) throw new Error("Tarefa não encontrada!");

        const tarefa = await res.json();

        if(confirm(`Tem certeza excluir a tarefa ${tarefa.nome}?`)){
            const deleteRes = await fetch(`https://localhost:7296/TimerTask/${tarefa.id}`, {
                method: "DELETE"
            });

            if (deleteRes.ok) {
                alert("Tarefa deletada com sucesso!");
                clearInterval(timer);
                timeInput.value = "";
                nomeInput.value = "";
                carregarTarefas();
            } else {
                alert("Erro ao deletar a tarefa!");
            }
        }
        
    } catch (error) {
        alert(error.message);
    }
}




///////////////////////////////// CRIAR tarefa//////////////////////



async function criarTarefa() {
    const timeInput = document.getElementById("timeNewInput");
    const nomeInput = document.getElementById("nomeNewInput");
    const novoNome = nomeInput.value.trim();
    const novoTempo = timeInput.value.trim();

    if (!novoNome || !novoTempo) {
        alert("Preencha todos os campos antes de criar a tarefa.");
        return;
    }

    const timeFormat = /^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$/;
    if (!timeFormat.test(novoTempo)) {
        alert("Formato de tempo inválido! Use HH:MM:SS.");
        return;
    }

    try {
        const createdTask = {
            nome: novoNome,
            horarioCronometrado: novoTempo
        };

        const createRes = await fetch(`https://localhost:7296/TimerTask`, {
            method: "POST", 
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(createdTask),
        });

        if (createRes.ok) {
            timeInput.value = "";
            nomeInput.value = "";
            alert("Tarefa criada com sucesso!");
            carregarTarefas();
        } else {
            alert("Erro ao criar a tarefa!");
        }        
    } catch (error) {
        alert(error.message);
    }

}


/////// INICIAR APP

async function startApp() {
    try {
        const res = await fetch("https://localhost:7296/TimerTask");
        if (!res.ok) throw new Error("Erro com a conexão!");
        document.getElementById("welcomeScreen").style.display = "none";
        document.getElementById("appContainer").style.display = "grid";
        document.getElementById("btnSalvar").disabled = true;
        carregarTarefas();
        iniciarIntervalo();
    } catch (error) {
        alert("Erro com a conexão " + error.message);
    }
}

function iniciarIntervalo() {
    setInterval(carregarTarefas, 60000);
}