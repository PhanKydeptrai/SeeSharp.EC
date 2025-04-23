using Application.Abstractions.Messaging;
using Domain.Entities.Vouchers;
using Domain.IRepositories;
using Domain.IRepositories.Vouchers;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.VoucherFeature.Commands.CreateNewVoucher;

internal sealed class CreateNewVoucherCommandHandler : ICommandHandler<CreateNewVoucherCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVoucherRepository _voucherRepository;

    public CreateNewVoucherCommandHandler(
        IUnitOfWork unitOfWork,
        IVoucherRepository voucherRepository)
    {
        _unitOfWork = unitOfWork;
        _voucherRepository = voucherRepository;
    }

    public async Task<Result> Handle(
        CreateNewVoucherCommand request,
        CancellationToken cancellationToken)
    {
        Voucher voucher;
        
        if (request.VoucherType.Equals("Direct", StringComparison.OrdinalIgnoreCase))
        {
            voucher = CreateDirectVoucher(request);
        }
        else if (request.VoucherType.Equals("Percentage", StringComparison.OrdinalIgnoreCase))
        {
            voucher = CreatePercentageVoucher(request);
        }
        else
        {
            return Result.Failure(VoucherError.InvalidVoucherType(request.VoucherType));
        }

        // Add voucher to repository
        await _voucherRepository.AddVoucher(voucher);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private Voucher CreateDirectVoucher(CreateNewVoucherCommand request)
    {
        return Voucher.NewDirectDiscountVoucher(
            VoucherName.FromString(request.VoucherName),
            VoucherCode.FromString(request.VoucherCode),
            MaximumDiscountAmount.FromDecimal(request.MaximumDiscountAmount),
            MinimumOrderAmount.FromDecimal(request.MinimumOrderAmount),
            request.StartDate,
            request.ExpiredDate,
            VoucherDescription.FromString(request.VoucherDescription));
    }

    private Voucher CreatePercentageVoucher(CreateNewVoucherCommand request)
    {
        return Voucher.NewPercentageDiscountVoucher(
            VoucherName.FromString(request.VoucherName),
            VoucherCode.FromString(request.VoucherCode),
            PercentageDiscount.FromInt(request.PercentageDiscount),
            MaximumDiscountAmount.FromDecimal(request.MaximumDiscountAmount),
            MinimumOrderAmount.FromDecimal(request.MinimumOrderAmount),
            request.StartDate,
            request.ExpiredDate,
            VoucherDescription.FromString(request.VoucherDescription));
    }
} 