using AutoMapper;
using VipProjectV0._1.Db;
using VipTest.Auth;
using VipTest.Localization;
using VipTest.reviews.dtos;
using VipTest.reviews.models;
using VipTest.reviews.Payloads;
using VipTest.reviews.utli;

namespace VipTest.reviews;

public interface IReviewService
{
    Task<(ReviewDto? dto,string? error)> CreateReviewsAsync(ReviewCreateForm form);
    Task<(ReviewDto? dto,string? error)> GetReviewByIdAsync(Guid id);
    Task<(List<ReviewDto>? dtos,int? totalCount,string? error)> GetAllReviewsAsync(ReviewFilterForm filterForm);
    
}

public class ReviewService:IReviewService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;

    public ReviewService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILocalizationService localize)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _localize = localize;
    }


    public async Task<(ReviewDto? dto, string? error)> CreateReviewsAsync(ReviewCreateForm form)
    {
        var customer = await _repositoryWrapper.CustomerRepository.GetById(form.CustomerId);
        if (customer == null)
        {
            return (null, _localize.GetLocalizedString("CustomerNotFound"));
        }

        if (form.DriverReview != null)
        {
            var driver=await _repositoryWrapper.DriverRepository.GetById(form.DriverReview.DriverId);
            if (driver == null)
            {
                return (null, _localize.GetLocalizedString("DriverNotFound"));
            }
            
            var driverReview = new DriverReview()
            {
                Comment = form.DriverReview.Comment,
                Rating = form.DriverReview.Rating,
                ReviewFor = ReviewFor.DriverReview,
                CustomerId = customer.Id,
                Customer = customer,
                DriverId = driver.Id,
                Driver = driver,
                DriverCode = driver.Id
            };
            
            driver.UpdateRating(form.DriverReview.Rating);
            
            await _repositoryWrapper.DriverReviewRepository.Add(driverReview);
            driverReview.Driver = driver;
            
            await _repositoryWrapper.DriverRepository.Update(driver, driver.Id); // Save the updated driver

            
        }
        if (form.VehicleReview != null)
        {
            var vehicle = await _repositoryWrapper.VehiclesRepository.GetById(form.VehicleReview.VehicleId);
            if (vehicle == null)
            {
                return (null, _localize.GetLocalizedString("VehicleNotFound"));
            }
            
            var vehicleReview = new VehicleReview()
            {
                Comment = form.VehicleReview.Comment,
                Rating = form.VehicleReview.Rating,
                ReviewFor = ReviewFor.RentedCarReview,
                CustomerId = customer.Id,
                Customer = customer,
                VehicleId = vehicle.Id,
                Vehicles = vehicle,
                VehicleCode = vehicle.Id
            };

            if (form.VehicleReview.CarRentalOrderId != null)
            {
                var carRentalOrder = await _repositoryWrapper.CarRentalOrderRepository.Get(
                    x => x.Id == form.VehicleReview.CarRentalOrderId);
                    
                if (carRentalOrder == null)
                {
                    return (null, _localize.GetLocalizedString("CarRentalOrderNotFound"));
                }
                
                carRentalOrder.IsReviewed = true;
                await _repositoryWrapper.CarRentalOrderRepository.Update(carRentalOrder, carRentalOrder.Id);
            }
            
            vehicle.UpdateRating(form.VehicleReview.Rating);
            await _repositoryWrapper.VehicleReviewRepository.Add(vehicleReview);
            vehicle.VehicleReviews.Add(vehicleReview);
            await _repositoryWrapper.VehiclesRepository.Update(vehicle, vehicle.Id); // Save the updated vehicle
            
        }
        
        if (form.RideReview != null)
        {
            var ride = await _repositoryWrapper.RideRepository.Get(x => x.Id == form.RideReview.RideId);
            if (ride == null)
            {
                return (null, _localize.GetLocalizedString("RideNotFound"));
            }
            
            var rideReview = new RideReview()
            {
                Comment = form.RideReview.Comment,
                Rating = form.RideReview.Rating,
                ReviewFor = ReviewFor.RideReview,
                CustomerId = customer.Id,
                Customer = customer,
                RideId = ride.Id,
                Ride = ride,
                RideCode = ride.Id
            };
            
            
            ride.UpdateRating(form.RideReview.Rating);
            await _repositoryWrapper.RideReviewRepository.Add(rideReview);
        }
        
        
        return (null, null);
    }

    public async Task<(ReviewDto? dto, string? error)> GetReviewByIdAsync(Guid id)
    {
        var review = await _repositoryWrapper.ReviewRepository.Get(x => x.Id == id);
        if (review == null)
        {
            return (null, _localize.GetLocalizedString("ReviewNotFound"));
        }
        
        var dto = _mapper.Map<ReviewDto>(review);
        return (dto, null);
    }

    public async Task<(List<ReviewDto>? dtos, int? totalCount, string? error)> GetAllReviewsAsync(ReviewFilterForm filterForm)
    {
        var (reviews, totalCount) = await _repositoryWrapper.ReviewRepository.GetAll<ReviewDto>(
            x=>(filterForm.CustomerId == null || x.CustomerId == filterForm.CustomerId) &&
               (filterForm.DriverId == null || x.DriverCode == filterForm.DriverId) &&
               (filterForm.VehicleId == null || x.VehicleCode == filterForm.VehicleId) &&
               (filterForm.RideId == null || x.RideCode == filterForm.RideId) &&
               (filterForm.ReviewFor == null || x.ReviewFor == filterForm.ReviewFor)
            , filterForm.PageNumber, filterForm.PageSize);
        return (reviews, totalCount, null);
    }
}