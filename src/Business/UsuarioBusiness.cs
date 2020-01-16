using AutoMapper;
using Business.Error;
using Business.Interface;
using Core.Arquitetura;
using ModelData.Model;
using ModelData.ViewModel;
using System;
using Util;

namespace Business
{
    public class UsuarioBusiness : IUsuarioBusiness
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioBusiness(IGenericRepository<Usuario> usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public void Alterar(long id, UsuarioViewModel viewModel)
        {
            var usuario = _usuarioRepository.ObterPorID(id);

            //Utilizar automapper futuramente para fazer o DE PARA
            usuario.NomeCompleto = viewModel.NomeCompleto;
            usuario.Email = viewModel.Email;

            _usuarioRepository.Editar(usuario);
        }

        public Usuario Criar(UsuarioViewModel viewModel)
        {
            var usuario = _mapper.Map<Usuario>(viewModel);
            usuario.DataDeCriacao = DateTime.Now;
            usuario.Status = (int)Enumeracao.ESituacao.Ativo;
            usuario.Senha = Constantes.SenhaDefault; 
            _usuarioRepository.Criar(usuario);

            return usuario;
        }

        public void TrocarSenha(long id, UsuarioSenhaViewModel viewModel)
        {
            var usuario = _usuarioRepository.ObterPorID(id);

            if (usuario.Senha != viewModel.SenhaAtual)
                throw new SenhaIncorretaException();

            usuario.Senha = viewModel.NovaSenha; //Nesse momento não irei gerar nenhum hash com salt por se tratar de um teste e apenas educativo
            _usuarioRepository.Editar(usuario);
        }
    }
}
