document.addEventListener('DOMContentLoaded', function() {
    const cronometros = document.querySelectorAll('.cronometro');

    cronometros.forEach(cronometro => {
        const horaEntrada = new Date(cronometro.dataset.entrada);
        const horaSalida = cronometro.dataset.salida ? new Date(cronometro.dataset.salida) : null;
        
        function actualizarCronometro() {
            const ahora = new Date();
            let diferencia;
            
            if (horaSalida) {
                diferencia = horaSalida - horaEntrada;
                cronometro.classList.add('finalizado');
            } else {
                diferencia = ahora - horaEntrada;
            }
            
            const horas = Math.floor(diferencia / 3600000);
            const minutos = Math.floor((diferencia % 3600000) / 60000);
            const segundos = Math.floor((diferencia % 60000) / 1000);
            
            cronometro.textContent = 
                `${horas.toString().padStart(2, '0')}:${minutos.toString().padStart(2, '0')}:${segundos.toString().padStart(2, '0')}`;
        }

        actualizarCronometro();
        if (!horaSalida) {
            setInterval(actualizarCronometro, 1000);
        }
    });
});