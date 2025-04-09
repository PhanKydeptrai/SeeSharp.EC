using Application.Abstractions.LinkService;
using Application.Abstractions.Messaging;
using Application.DTOs.Voucher;
using Application.Features.Pages;
using Application.IServices;
using SharedKernel;
using SharedKernel.Constants;

namespace Application.Features.VoucherFeature.Queries.GetAllVoucher;

internal sealed class GetAllVoucherQueryHandler : IQueryHandler<GetAllVoucherQuery, PagedList<VoucherResponse>>
{
    private readonly IVoucherQueryServices _voucherQueryServices;
    private readonly ILinkServices _linkServices;

    public GetAllVoucherQueryHandler(
        IVoucherQueryServices voucherQueryServices,
        ILinkServices linkServices)
    {
        _voucherQueryServices = voucherQueryServices;
        _linkServices = linkServices;
    }

    public async Task<Result<PagedList<VoucherResponse>>> Handle(
        GetAllVoucherQuery request, 
        CancellationToken cancellationToken)
    {
        var pagedList = await _voucherQueryServices.GetAllVouchers(
            request.voucherTypeFilter,
            request.statusFilter,
            request.searchTerm,
            request.sortColumn,
            request.sortOrder,
            request.page ?? 1,
            request.pageSize ?? 10,
            cancellationToken);

        AddLinks(request, pagedList);
        return Result.Success(pagedList);
    }

    private void AddLinks(GetAllVoucherQuery request, PagedList<VoucherResponse> pagedList)
    {
        pagedList.Links.Add(_linkServices.Generate(
            EndpointName.Voucher.GetAll,
            new
            {
                voucherTypeFilter = request.voucherTypeFilter,
                statusFilter = request.statusFilter,
                searchTerm = request.searchTerm,
                sortColumn = request.sortColumn,
                sortOrder = request.sortOrder,
                page = request.page,
                pageSize = request.pageSize
            },
            "self",
            EndpointMethod.GET));

        if (pagedList.HaspreviousPage)
        {
            pagedList.Links.Add(_linkServices.Generate(
                EndpointName.Voucher.GetAll,
                new
                {
                    voucherTypeFilter = request.voucherTypeFilter,
                    statusFilter = request.statusFilter,
                    searchTerm = request.searchTerm,
                    sortColumn = request.sortColumn,
                    sortOrder = request.sortOrder,
                    page = request.page - 1,
                    pageSize = request.pageSize
                },
                "previous-page",
                EndpointMethod.GET));
        }

        if (pagedList.HasNextPage)
        {
            pagedList.Links.Add(_linkServices.Generate(
                EndpointName.Voucher.GetAll,
                new
                {
                    voucherTypeFilter = request.voucherTypeFilter,
                    statusFilter = request.statusFilter,
                    searchTerm = request.searchTerm,
                    sortColumn = request.sortColumn,
                    sortOrder = request.sortOrder,
                    page = request.page + 1,
                    pageSize = request.pageSize
                },
                "next-page",
                EndpointMethod.GET));
        }
    }
} 