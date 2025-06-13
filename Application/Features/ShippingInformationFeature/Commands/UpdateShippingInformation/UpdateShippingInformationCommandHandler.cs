using Application.Abstractions.Messaging;
using Application.IServices;
using Domain.Entities.ShippingInformations;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.ShippingInformations;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.ShippingInformationFeature.Commands.UpdateShippingInformation;

internal sealed class UpdateShippingInformationCommandHandler : ICommandHandler<UpdateShippingInformationCommand>
{
    private readonly IShippingInformationRepository _shippingInformationRepository;
    private readonly IShippingInformationQueryServices _shippingInformationService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateShippingInformationCommandHandler(
        IShippingInformationRepository shippingInformationRepository,
        IUnitOfWork unitOfWork,
        IShippingInformationQueryServices shippingInformationService)
    {
        _shippingInformationRepository = shippingInformationRepository;
        _unitOfWork = unitOfWork;
        _shippingInformationService = shippingInformationService;
    }

    public async Task<Result> Handle(UpdateShippingInformationCommand request, CancellationToken cancellationToken)
    {
        var shippingInformationId = ShippingInformationId.FromGuid(request.ShippingInformationId);
        var shippingInformation = await _shippingInformationRepository.GetShippingInformationById(shippingInformationId);

        if (shippingInformation is null) //Kiểm tra xem thông tin giao hàng có tồn tại hay không
        {
            return Result.Failure(ShippingInformationError.NotFound(shippingInformationId));
        }

        bool isDefault = await _shippingInformationService.IsThisShippingInformationDefault(shippingInformationId);

        if (isDefault && shippingInformation.IsDefault == IsDefault.False)
        {
            return Result.Failure(ShippingInformationError.CannotUnsetDefaultShippingInformation(shippingInformationId));
        }

        if (shippingInformation.IsDefault == IsDefault.True)
        {
            var oldDefaultShippingInformation = await _shippingInformationRepository.GetDefaultShippingInformation(
            shippingInformation.CustomerId);

            oldDefaultShippingInformation!.UnsetDefault();
        }

        shippingInformation.Update(
            FullName.FromString(request.FullName),
            PhoneNumber.FromString(request.PhoneNumber),
            IsDefault.FromBoolean(request.IsDefault),
            SpecificAddress.FromString(request.SpecificAddress),
            Province.FromString(request.Province),
            District.FromString(request.District),
            Ward.FromString(request.Ward));

        await _unitOfWork.SaveChangesAsync();
        return Result.Success();

    }

    #region Backup Code
    // public async Task<Result> Handle(UpdateShippingInformationCommand request, CancellationToken cancellationToken)
    // {
    //     var shippingInformationId = ShippingInformationId.FromGuid(request.ShippingInformationId);
    //     var shippingInformation = await _shippingInformationRepository.GetShippingInformationById(shippingInformationId);

    //     if (shippingInformation is null) //Kiểm tra xem thông tin giao hàng có tồn tại hay không
    //     {
    //         return Result.Failure(ShippingInformationError.NotFound(shippingInformationId));
    //     }

    //     if (await _shippingInformationService.IsThisShippingInformationDefault(shippingInformationId))
    //     {
    //         if (shippingInformation.IsDefault == IsDefault.True)
    //         {
    //             shippingInformation.Update(
    //                 FullName.FromString(request.FullName),
    //                 PhoneNumber.FromString(request.PhoneNumber),
    //                 IsDefault.FromBoolean(request.IsDefault),
    //                 SpecificAddress.FromString(request.SpecificAddress),
    //                 Province.FromString(request.Province),
    //                 District.FromString(request.District),
    //                 Ward.FromString(request.Ward));

    //             await _unitOfWork.SaveChangesAsync();
    //             return Result.Success();
    //         }

    //         return Result.Failure(ShippingInformationError.CannotUnsetDefaultShippingInformation(shippingInformationId));
    //     }

    //     if (shippingInformation.IsDefault == IsDefault.False)
    //     {
    //         shippingInformation.Update(
    //             FullName.FromString(request.FullName),
    //             PhoneNumber.FromString(request.PhoneNumber),
    //             IsDefault.FromBoolean(request.IsDefault),
    //             SpecificAddress.FromString(request.SpecificAddress),
    //             Province.FromString(request.Province),
    //             District.FromString(request.District),
    //             Ward.FromString(request.Ward));

    //         await _unitOfWork.SaveChangesAsync();
    //         return Result.Success();
    //     }

    //     var oldDefaultShippingInformation = await _shippingInformationRepository.GetDefaultShippingInformation(
    //         shippingInformation.CustomerId);

    //     oldDefaultShippingInformation!.UnsetDefault();

    //     shippingInformation.Update(
    //         FullName.FromString(request.FullName),
    //         PhoneNumber.FromString(request.PhoneNumber),
    //         IsDefault.FromBoolean(request.IsDefault),
    //         SpecificAddress.FromString(request.SpecificAddress),
    //         Province.FromString(request.Province),
    //         District.FromString(request.District),
    //         Ward.FromString(request.Ward));

    //     await _unitOfWork.SaveChangesAsync();
    //     return Result.Success();

    // }
    #endregion
}