INCLUDE(${CMAKE_ROOT}/Modules/CMakeFindFrameworks.cmake)

FIND_PATH(PIXYZ_INCLUDE_PATH OptimizeSDKTypes.h
  ${PIXYZ_SDK_DIR}/include
)

FIND_LIBRARY(PIXYZ_OPTIMIZE_LIBRARY
  NAMES PiXYZOptimizeSDK
  PATHS
  ${PIXYZ_SDK_DIR}/lib
)