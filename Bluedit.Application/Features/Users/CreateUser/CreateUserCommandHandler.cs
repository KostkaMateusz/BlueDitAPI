
using AutoMapper;
using Bluedit.Application.Contracts.Persistence;
using MediatR;

namespace Bluedit.Application.Features.Users.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand,CreateUserCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public CreateUserCommandHandler(IMapper mapper,IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
