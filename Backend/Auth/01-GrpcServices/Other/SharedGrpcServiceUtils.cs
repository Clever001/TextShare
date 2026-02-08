namespace Auth.GrpcService.Other;

public class SharedGrpcServiceUtils {
    public static void LogException<T>(ILogger<T> logger, Exception exception) {
        logger.LogError(exception, "BusinessLoginException if RegisterUser.");
    }
}