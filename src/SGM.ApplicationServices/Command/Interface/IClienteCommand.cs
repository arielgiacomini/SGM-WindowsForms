﻿using SGM.Domain.Entities;

namespace SGM.ApplicationServices.Command.Interface
{
    public interface IClienteCommand
    {
        void SalvarCliente(Cliente cliente);
    }
}
